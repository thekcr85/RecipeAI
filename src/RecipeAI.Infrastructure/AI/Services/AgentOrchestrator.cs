using System.Text.Json;
using RecipeAI.Domain.Entities;
using RecipeAI.Domain.Enums;
using RecipeAI.Domain.Interfaces;
using RecipeAI.Infrastructure.Data;

namespace RecipeAI.Infrastructure.AI.Services;

/// <summary>
/// Orchestrates multiple AI agents for meal planning with iterative refinement
/// </summary>
/// <param name="plannerService">Meal planning agent service</param>
/// <param name="criticService">Nutrition critic service</param>
/// <param name="budgetService">Budget optimizer service</param>
/// <param name="context">Database context</param>
public class AgentOrchestrator(
	MealPlanningAgentService plannerService,
	NutritionCriticService criticService,
	BudgetOptimizerService budgetService,
	RecipeAIDbContext context)
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

		await context.PlanningSessions.AddAsync(session, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);

		var iterationLogs = new List<string>();
		string currentPlanJson;
		bool approved = false;

		try
		{
			// Initial generation
			currentPlanJson = await plannerService.GenerateMealPlanAsync(
				dietType, numberOfDays, targetCalories, budgetLimit, cancellationToken);

			iterationLogs.Add("Iteration 1: Initial plan generated");
			session.IterationCount = 1;

			// Iterative refinement loop
			for (int i = 0; i < MaxIterations && !approved; i++)
			{
				// Nutrition validation
				var nutritionFeedback = await criticService.ValidateNutritionAsync(
					currentPlanJson, targetCalories, cancellationToken);

				iterationLogs.Add($"Iteration {i + 1}: Nutrition feedback - {nutritionFeedback}");

				// Budget optimization
				var budgetFeedback = await budgetService.OptimizeBudgetAsync(
					currentPlanJson, budgetLimit, cancellationToken);

				iterationLogs.Add($"Iteration {i + 1}: Budget feedback - {budgetFeedback}");

				// Check if approved
				approved = IsApproved(nutritionFeedback, budgetFeedback);

				if (!approved && i < MaxIterations - 1)
				{
					var combinedFeedback = $"Nutrition: {nutritionFeedback}\nBudget: {budgetFeedback}";

					currentPlanJson = await plannerService.RefineMealPlanAsync(
						currentPlanJson, combinedFeedback, cancellationToken);

					session.IterationCount++;
					iterationLogs.Add($"Iteration {i + 2}: Plan refined");
				}
			}

			// Parse and save final meal plan
			var mealPlan = ParseMealPlan(currentPlanJson, dietType, numberOfDays, targetCalories);
			mealPlan.PlanningSessionId = session.Id;

			await context.MealPlans.AddAsync(mealPlan, cancellationToken);

			session.Complete();
			session.IterationLogs = JsonSerializer.Serialize(iterationLogs);

			await context.SaveChangesAsync(cancellationToken);

			return mealPlan;
		}
		catch
		{
			session.Fail();
			session.IterationLogs = JsonSerializer.Serialize(iterationLogs);
			await context.SaveChangesAsync(cancellationToken);
			throw;
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