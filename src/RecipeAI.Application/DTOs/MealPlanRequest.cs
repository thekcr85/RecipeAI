using RecipeAI.Domain.Enums;

namespace RecipeAI.Application.DTOs;

/// <summary>
/// Request for generating a meal plan
/// </summary>
public class MealPlanRequest
{
	/// <summary>
	/// Gets or sets the diet type
	/// </summary>
	public DietType DietType { get; set; }

	/// <summary>
	/// Gets or sets the number of days to plan
	/// </summary>
	public int NumberOfDays { get; set; }

	/// <summary>
	/// Gets or sets the daily calorie target
	/// </summary>
	public int TargetCalories { get; set; }

	/// <summary>
	/// Gets or sets the budget limit in PLN
	/// </summary>
	public decimal BudgetLimit { get; set; }
}
