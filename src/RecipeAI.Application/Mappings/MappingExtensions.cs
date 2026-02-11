using RecipeAI.Application.DTOs;
using RecipeAI.Domain.Entities;
using System.Text.Json;

namespace RecipeAI.Application.Mappings;

/// <summary>
/// Extension methods for mapping between entities and DTOs
/// </summary>
public static class MappingExtensions
{
	/// <summary>
	/// Maps a MealPlan entity to MealPlanResponse DTO
	/// </summary>
	/// <param name="mealPlan">The meal plan entity</param>
	/// <returns>The meal plan response DTO</returns>
	public static MealPlanResponse ToResponse(this MealPlan mealPlan)
	{
		return new MealPlanResponse
		{
			Id = mealPlan.Id,
			Name = mealPlan.Name,
			DietType = mealPlan.DietType,
			NumberOfDays = mealPlan.NumberOfDays,
			TargetCalories = mealPlan.TargetCalories,
			TotalCost = mealPlan.TotalCost,
			CreatedAt = mealPlan.CreatedAt,
			Recipes = mealPlan.Recipes.Select(r => r.ToDto()).ToList(),
			NutritionSummary = CalculateNutritionSummary(mealPlan),
			IterationInfo = mealPlan.PlanningSession?.ToIterationInfo()
		};
	}

	/// <summary>
	/// Maps a Recipe entity to RecipeDto
	/// </summary>
	/// <param name="recipe">The recipe entity</param>
	/// <returns>The recipe DTO</returns>
	public static RecipeDto ToDto(this Recipe recipe)
	{
		return new RecipeDto
		{
			Id = recipe.Id,
			Name = recipe.Name,
			MealType = recipe.MealType,
			DayNumber = recipe.DayNumber,
			Instructions = recipe.Instructions,
			PreparationTimeMinutes = recipe.PreparationTimeMinutes,
			Calories = recipe.Calories,
			ProteinGrams = recipe.ProteinGrams,
			CarbsGrams = recipe.CarbsGrams,
			FatGrams = recipe.FatGrams,
			EstimatedCost = recipe.EstimatedCost,
			Ingredients = recipe.Ingredients.Select(i => i.ToDto()).ToList()
		};
	}

	/// <summary>
	/// Maps an Ingredient entity to IngredientDto
	/// </summary>
	/// <param name="ingredient">The ingredient entity</param>
	/// <returns>The ingredient DTO</returns>
	public static IngredientDto ToDto(this Ingredient ingredient)
	{
		return new IngredientDto
		{
			Id = ingredient.Id,
			Name = ingredient.Name,
			Quantity = ingredient.Quantity,
			Unit = ingredient.Unit,
			Cost = ingredient.Cost
		};
	}

	/// <summary>
	/// Maps a PlanningSession entity to AgentIterationInfo DTO
	/// </summary>
	/// <param name="session">The planning session entity</param>
	/// <returns>The iteration info DTO</returns>
	public static AgentIterationInfo ToIterationInfo(this PlanningSession session)
	{
		var logs = new List<string>();
		if (!string.IsNullOrEmpty(session.IterationLogs))
		{
			try
			{
				logs = JsonSerializer.Deserialize<List<string>>(session.IterationLogs) ?? [];
			}
			catch
			{
				logs = [session.IterationLogs];
			}
		}

		return new AgentIterationInfo
		{
			SessionId = session.Id,
			Status = session.Status,
			IterationCount = session.IterationCount,
			StartedAt = session.StartedAt,
			CompletedAt = session.CompletedAt,
			IterationLogs = logs
		};
	}

	private static NutritionSummary CalculateNutritionSummary(MealPlan mealPlan)
	{
		var totalCalories = mealPlan.Recipes.Sum(r => r.Calories);
		var totalProtein = mealPlan.Recipes.Sum(r => r.ProteinGrams);
		var totalCarbs = mealPlan.Recipes.Sum(r => r.CarbsGrams);
		var totalFat = mealPlan.Recipes.Sum(r => r.FatGrams);

		var proteinCalories = totalProtein * 4;
		var carbsCalories = totalCarbs * 4;
		var fatCalories = totalFat * 9;
		var totalMacroCalories = proteinCalories + carbsCalories + fatCalories;

		return new NutritionSummary
		{
			TotalCalories = totalCalories,
			AverageDailyCalories = mealPlan.NumberOfDays > 0 ? totalCalories / mealPlan.NumberOfDays : 0,
			TotalProteinGrams = totalProtein,
			TotalCarbsGrams = totalCarbs,
			TotalFatGrams = totalFat,
			ProteinPercentage = totalMacroCalories > 0 ? Math.Round(proteinCalories / totalMacroCalories * 100, 1) : 0,
			CarbsPercentage = totalMacroCalories > 0 ? Math.Round(carbsCalories / totalMacroCalories * 100, 1) : 0,
			FatPercentage = totalMacroCalories > 0 ? Math.Round(fatCalories / totalMacroCalories * 100, 1) : 0
		};
	}
}
