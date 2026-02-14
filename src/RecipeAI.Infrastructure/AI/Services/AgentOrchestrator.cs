using RecipeAI.Domain.Entities;
using RecipeAI.Domain.Enums;
using RecipeAI.Infrastructure.AI.Agents;
using RecipeAI.Infrastructure.AI.Helpers;
using RecipeAI.Infrastructure.Data;
using System.Text.Json;

namespace RecipeAI.Infrastructure.AI.Services;

/// <summary>
/// Orchestrates multiple AI agents for meal planning with iterative refinement
/// </summary>
public class AgentOrchestrator
{
	private readonly MealPlanningAgent _plannerAgent;
	private readonly NutritionCriticAgent _criticAgent;
	private readonly BudgetOptimizerAgent _budgetAgent;
	private readonly RecipeAIDbContext _context;
	private const int MaxIterations = 3;

	public AgentOrchestrator(
		MealPlanningAgent plannerAgent,
		NutritionCriticAgent criticAgent,
		BudgetOptimizerAgent budgetAgent,
		RecipeAIDbContext context)
	{
		_plannerAgent = plannerAgent;
		_criticAgent = criticAgent;
		_budgetAgent = budgetAgent;
		_context = context;
	}

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

		await _context.PlanningSessions.AddAsync(session, cancellationToken);
		await _context.SaveChangesAsync(cancellationToken);

		var iterationLogs = new List<string>();
		string currentPlanJson;
		bool approved = false;

		try
		{
			// Initial generation using Planner Agent
			var rawPlan = await _plannerAgent.GenerateMealPlanAsync(
				dietType, numberOfDays, targetCalories, budgetLimit, cancellationToken);
			
			// Clean and validate JSON response
			currentPlanJson = JsonResponseHelper.CleanJsonResponse(rawPlan);
			
			// Debug logging
			Console.WriteLine($"[Orchestrator] Raw response length: {rawPlan?.Length ?? 0}");
			Console.WriteLine($"[Orchestrator] Cleaned JSON length: {currentPlanJson?.Length ?? 0}");
			
			if (!JsonResponseHelper.IsValidJson(currentPlanJson))
			{
				Console.WriteLine($"[Orchestrator] ERROR: Invalid JSON received");
				Console.WriteLine($"[Orchestrator] Last 300 chars: {currentPlanJson?[Math.Max(0, (currentPlanJson?.Length ?? 0) - 300)..]}");
				throw new InvalidOperationException(
					"AI returned incomplete or invalid JSON. " +
					"This usually means MaxTokens is too low. " +
					"Try: 1) Reduce number of days, 2) Increase MaxTokens to 6000+, or 3) Use gpt-4o-mini for faster responses.");
			}

			iterationLogs.Add($"Iteration 1: Initial plan generated ({currentPlanJson.Length} chars)");
			session.IterationCount = 1;

			// Iterative refinement loop with Critic and Optimizer agents
			for (int i = 0; i < MaxIterations && !approved; i++)
			{
				// Nutrition validation using Critic Agent with diet type
				var rawNutritionFeedback = await _criticAgent.ValidateNutritionAsync(
					currentPlanJson, targetCalories, dietType.ToString(), cancellationToken);
				var nutritionFeedback = JsonResponseHelper.CleanJsonResponse(rawNutritionFeedback);

				iterationLogs.Add($"Iteration {i + 1}: Nutrition feedback - {(nutritionFeedback.Length > 200 ? nutritionFeedback[..200] + "..." : nutritionFeedback)}");

				// Budget optimization using Budget Optimizer Agent
				var rawBudgetFeedback = await _budgetAgent.OptimizeBudgetAsync(
					currentPlanJson, budgetLimit, cancellationToken);
				var budgetFeedback = JsonResponseHelper.CleanJsonResponse(rawBudgetFeedback);

				iterationLogs.Add($"Iteration {i + 1}: Budget feedback - {(budgetFeedback.Length > 200 ? budgetFeedback[..200] + "..." : budgetFeedback)}");

				// Check if approved by both agents
				approved = IsApproved(nutritionFeedback, budgetFeedback);

				if (!approved && i < MaxIterations - 1)
				{
					// Store current plan as backup
					var previousPlanJson = currentPlanJson;
					
					// Simplify feedback to reduce token usage in refinement
					var simplifiedFeedback = SimplifyFeedback(nutritionFeedback, budgetFeedback);
					
					try
					{
						var rawRefinedPlan = await _plannerAgent.RefineMealPlanAsync(
							currentPlanJson, simplifiedFeedback, cancellationToken);
						var refinedPlanJson = JsonResponseHelper.CleanJsonResponse(rawRefinedPlan);
						
						// Validate refined plan
						if (JsonResponseHelper.IsValidJson(refinedPlanJson))
						{
							currentPlanJson = refinedPlanJson;
							session.IterationCount++;
							iterationLogs.Add($"Iteration {i + 2}: Plan refined successfully ({currentPlanJson.Length} chars)");
						}
						else
						{
							Console.WriteLine($"[Orchestrator] WARNING: Refined plan iteration {i + 2} is invalid, keeping previous version");
							iterationLogs.Add($"Iteration {i + 2}: Refinement failed (invalid JSON), using previous plan");
							// Keep previous plan and exit refinement loop
							approved = true;
						}
					}
					catch (Exception ex)
					{
						Console.WriteLine($"[Orchestrator] ERROR during refinement: {ex.Message}");
						iterationLogs.Add($"Iteration {i + 2}: Refinement failed ({ex.Message}), using previous plan");
						// Keep previous plan and exit refinement loop
						approved = true;
					}
				}
			}

			// Parse and save final meal plan
			var mealPlan = ParseMealPlan(currentPlanJson, dietType, numberOfDays, targetCalories);
			mealPlan.PlanningSessionId = session.Id;

			await _context.MealPlans.AddAsync(mealPlan, cancellationToken);

			session.Complete();
			session.IterationLogs = JsonSerializer.Serialize(iterationLogs);

			await _context.SaveChangesAsync(cancellationToken);

			Console.WriteLine($"[Orchestrator] SUCCESS: Meal plan created with {mealPlan.Recipes.Count} recipes");
			return mealPlan;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"[Orchestrator] EXCEPTION: {ex.Message}");
			session.Fail();
			session.IterationLogs = JsonSerializer.Serialize(iterationLogs);
			await _context.SaveChangesAsync(cancellationToken);
			throw;
		}
	}

	/// <summary>
	/// Simplifies feedback to reduce token consumption during refinement
	/// </summary>
	private static string SimplifyFeedback(string nutritionFeedback, string budgetFeedback)
	{
		try
		{
			var nutritionDoc = JsonDocument.Parse(nutritionFeedback);
			var budgetDoc = JsonDocument.Parse(budgetFeedback);

			var nutritionApproved = nutritionDoc.RootElement.GetProperty("approved").GetBoolean();
			var budgetApproved = budgetDoc.RootElement.GetProperty("withinBudget").GetBoolean();

			var feedback = "Issues to fix:\n";
			
			if (!nutritionApproved)
			{
				if (nutritionDoc.RootElement.TryGetProperty("suggestions", out var suggestions))
				{
					feedback += "Nutrition: ";
					foreach (var suggestion in suggestions.EnumerateArray())
					{
						feedback += suggestion.GetString() + "; ";
					}
					feedback += "\n";
				}
			}

			if (!budgetApproved)
			{
				if (budgetDoc.RootElement.TryGetProperty("suggestions", out var suggestions))
				{
					feedback += "Budget: ";
					foreach (var suggestion in suggestions.EnumerateArray())
					{
						feedback += suggestion.GetString() + "; ";
					}
				}
			}

			return feedback;
		}
		catch
		{
			// Fallback to simple feedback
			return $"Nutrition: {nutritionFeedback}\nBudget: {budgetFeedback}";
		}
	}

	private static bool IsApproved(string nutritionFeedback, string budgetFeedback)
	{
		try
		{
			var nutritionDoc = JsonDocument.Parse(nutritionFeedback);
			var budgetDoc = JsonDocument.Parse(budgetFeedback);

			var nutritionApproved = nutritionDoc.RootElement.GetProperty("approved").GetBoolean();
			var budgetApproved = budgetDoc.RootElement.GetProperty("withinBudget").GetBoolean();

			return nutritionApproved && budgetApproved;
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
				var ingredient = new Ingredient
				{
					Name = ingredientJson.GetProperty("name").GetString()!,
					Quantity = ingredientJson.GetProperty("quantity").GetDecimal(),
					Unit = ingredientJson.GetProperty("unit").GetString()!,
					Cost = ingredientJson.GetProperty("cost").GetDecimal()
				};

				recipe.Ingredients.Add(ingredient);
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
