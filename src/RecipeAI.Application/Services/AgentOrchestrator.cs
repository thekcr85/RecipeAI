using RecipeAI.Application.Helpers;
using RecipeAI.Application.Interfaces;
using RecipeAI.Application.Interfaces.Agents;
using RecipeAI.Domain.Entities;
using RecipeAI.Domain.Enums;
using RecipeAI.Domain.Interfaces;
using System.Text.Json;

namespace RecipeAI.Application.Services;

/// <summary>
/// Orchestrates multiple AI agents for meal planning with iterative refinement
/// </summary>
public class AgentOrchestrator(
	IMealPlanningAgent plannerAgent,
	INutritionCriticAgent criticAgent,
	IBudgetOptimizerAgent budgetAgent,
	IPlanningSessionRepository sessionRepository) : IAgentOrchestrator
{
	private const int MaxIterations = 3;

	/// <summary>
	/// Orchestrates the meal planning process with multi-agent collaboration
	/// </summary>
	public async Task<MealPlan> CreateMealPlanAsync(
		DietType dietType,
		int numberOfDays,
		int targetCalories,
		decimal budgetLimit,
		CancellationToken cancellationToken = default)
	{
		var session = new PlanningSession
		{
			UserRequest = $"Diet: {dietType}, Days: {numberOfDays}, Calories: {targetCalories}, Budget: {budgetLimit}",
			StartedAt = DateTime.UtcNow,
			Status = SessionStatus.InProgress,
			IterationCount = 0
		};

		await sessionRepository.AddAsync(session, cancellationToken);
		await sessionRepository.SaveChangesAsync(cancellationToken);

		var iterationLogs = new List<string>();
		string currentPlanJson;
		bool approved = false;

		try
		{
			// Initial generation using Planner Agent
			var rawPlan = await plannerAgent.GenerateMealPlanAsync(
				dietType, numberOfDays, targetCalories, budgetLimit, cancellationToken);

			currentPlanJson = JsonResponseHelper.CleanJsonResponse(rawPlan);

			if (!JsonResponseHelper.IsValidJson(currentPlanJson))
				throw new InvalidOperationException(
					"AI returned incomplete or invalid JSON. " +
					"This usually means MaxTokens is too low. " +
					"Try: 1) Reduce number of days, 2) Increase MaxTokens to 6000+, or 3) Use gpt-4o-mini for faster responses.");

			iterationLogs.Add($"Iteration 1: Initial plan generated ({currentPlanJson.Length} chars)");
			session.IterationCount = 1;

			// Iterative refinement loop with Critic and Optimizer agents
			for (int i = 0; i < MaxIterations && !approved; i++)
			{
				var rawNutritionFeedback = await criticAgent.ValidateNutritionAsync(
					currentPlanJson, targetCalories, dietType.ToString(), cancellationToken);
				var nutritionFeedback = JsonResponseHelper.CleanJsonResponse(rawNutritionFeedback);

				iterationLogs.Add($"Iteration {i + 1}: Nutrition feedback - {(nutritionFeedback.Length > 200 ? nutritionFeedback[..200] + "..." : nutritionFeedback)}");

				var rawBudgetFeedback = await budgetAgent.OptimizeBudgetAsync(
					currentPlanJson, budgetLimit, cancellationToken);
				var budgetFeedback = JsonResponseHelper.CleanJsonResponse(rawBudgetFeedback);

				iterationLogs.Add($"Iteration {i + 1}: Budget feedback - {(budgetFeedback.Length > 200 ? budgetFeedback[..200] + "..." : budgetFeedback)}");

				approved = IsApproved(nutritionFeedback, budgetFeedback);

				if (!approved && i < MaxIterations - 1)
				{
					var simplifiedFeedback = SimplifyFeedback(nutritionFeedback, budgetFeedback);

					try
					{
						var rawRefinedPlan = await plannerAgent.RefineMealPlanAsync(
							currentPlanJson, simplifiedFeedback, cancellationToken);
						var refinedPlanJson = JsonResponseHelper.CleanJsonResponse(rawRefinedPlan);

						if (JsonResponseHelper.IsValidJson(refinedPlanJson))
						{
							currentPlanJson = refinedPlanJson;
							session.IterationCount++;
							iterationLogs.Add($"Iteration {i + 2}: Plan refined successfully ({currentPlanJson.Length} chars)");
						}
						else
						{
							iterationLogs.Add($"Iteration {i + 2}: Refinement failed (invalid JSON), using previous plan");
							approved = true;
						}
					}
					catch (Exception ex)
					{
						iterationLogs.Add($"Iteration {i + 2}: Refinement failed ({ex.Message}), using previous plan");
						approved = true;
					}
				}
			}

			var mealPlan = ParseMealPlan(currentPlanJson, dietType, numberOfDays, targetCalories);
			mealPlan.PlanningSessionId = session.Id;

			session.Complete();
			session.IterationLogs = JsonSerializer.Serialize(iterationLogs);
			session.FinalMealPlan = mealPlan;

			await sessionRepository.SaveChangesAsync(cancellationToken);

			return mealPlan;
		}
		catch (Exception)
		{
			session.Fail();
			session.IterationLogs = JsonSerializer.Serialize(iterationLogs);
			await sessionRepository.SaveChangesAsync(cancellationToken);
			throw;
		}
	}

	private static string SimplifyFeedback(string nutritionFeedback, string budgetFeedback)
	{
		try
		{
			var nutritionDoc = JsonDocument.Parse(nutritionFeedback);
			var budgetDoc = JsonDocument.Parse(budgetFeedback);

			var nutritionApproved = nutritionDoc.RootElement.GetProperty("approved").GetBoolean();
			var budgetApproved = budgetDoc.RootElement.GetProperty("withinBudget").GetBoolean();

			var feedback = "Issues to fix:\n";

			if (!nutritionApproved && nutritionDoc.RootElement.TryGetProperty("suggestions", out var nutritionSuggestions))
			{
				feedback += "Nutrition: ";
				foreach (var suggestion in nutritionSuggestions.EnumerateArray())
					feedback += suggestion.GetString() + "; ";
				feedback += "\n";
			}

			if (!budgetApproved && budgetDoc.RootElement.TryGetProperty("suggestions", out var budgetSuggestions))
			{
				feedback += "Budget: ";
				foreach (var suggestion in budgetSuggestions.EnumerateArray())
					feedback += suggestion.GetString() + "; ";
			}

			return feedback;
		}
		catch
		{
			return $"Nutrition: {nutritionFeedback}\nBudget: {budgetFeedback}";
		}
	}

	private static bool IsApproved(string nutritionFeedback, string budgetFeedback)
	{
		try
		{
			var nutritionDoc = JsonDocument.Parse(nutritionFeedback);
			var budgetDoc = JsonDocument.Parse(budgetFeedback);

			return nutritionDoc.RootElement.GetProperty("approved").GetBoolean()
				&& budgetDoc.RootElement.GetProperty("withinBudget").GetBoolean();
		}
		catch
		{
			return false;
		}
	}

	private static MealPlan ParseMealPlan(string json, DietType dietType, int days, int calories)
	{
		var doc = JsonDocument.Parse(json);
		var recipesJson = doc.RootElement.GetProperty("mealPlan").GetProperty("recipes");

		var recipes = new List<Recipe>();
		decimal totalCost = 0;

		foreach (var recipeJson in recipesJson.EnumerateArray())
		{
			var recipe = new Recipe
			{
				Name = recipeJson.GetProperty("name").GetString()!,
				MealType = Enum.Parse<MealType>(recipeJson.GetProperty("mealType").GetString()!),
				DayNumber = recipeJson.GetProperty("dayNumber").GetInt32(),
				Instructions = recipeJson.GetProperty("instructions").GetString()!,
				PreparationTimeMinutes = recipeJson.GetProperty("preparationTimeMinutes").GetInt32(),
				Calories = recipeJson.GetProperty("calories").GetInt32(),
				ProteinGrams = recipeJson.GetProperty("proteinGrams").GetDecimal(),
				CarbsGrams = recipeJson.GetProperty("carbsGrams").GetDecimal(),
				FatGrams = recipeJson.GetProperty("fatGrams").GetDecimal(),
				EstimatedCost = recipeJson.GetProperty("estimatedCost").GetDecimal(),
				Ingredients = []
			};

			foreach (var ingredientJson in recipeJson.GetProperty("ingredients").EnumerateArray())
			{
				recipe.Ingredients.Add(new Ingredient
				{
					Name = ingredientJson.GetProperty("name").GetString()!,
					Quantity = ingredientJson.GetProperty("quantity").GetDecimal(),
					Unit = ingredientJson.GetProperty("unit").GetString()!,
					Cost = ingredientJson.GetProperty("cost").GetDecimal()
				});
			}

			totalCost += recipe.EstimatedCost;
			recipes.Add(recipe);
		}

		return new MealPlan
		{
			Name = $"Plan {dietType} - {days} dni",
			DietType = dietType,
			NumberOfDays = days,
			TargetCalories = calories,
			TotalCost = totalCost,
			Recipes = recipes,
			CreatedAt = DateTime.UtcNow
		};
	}
}