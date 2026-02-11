using RecipeAI.Domain.Enums;

namespace RecipeAI.Application.DTOs;

/// <summary>
/// Response containing meal plan details
/// </summary>
public class MealPlanResponse
{
	/// <summary>
	/// Gets or sets the meal plan identifier
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the meal plan name
	/// </summary>
	public required string Name { get; set; }

	/// <summary>
	/// Gets or sets the diet type
	/// </summary>
	public DietType DietType { get; set; }

	/// <summary>
	/// Gets or sets the number of days
	/// </summary>
	public int NumberOfDays { get; set; }

	/// <summary>
	/// Gets or sets the target calories
	/// </summary>
	public int TargetCalories { get; set; }

	/// <summary>
	/// Gets or sets the total cost
	/// </summary>
	public decimal TotalCost { get; set; }

	/// <summary>
	/// Gets or sets when the plan was created
	/// </summary>
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets the recipes
	/// </summary>
	public List<RecipeDto> Recipes { get; set; } = [];

	/// <summary>
	/// Gets or sets the nutrition summary
	/// </summary>
	public NutritionSummary? NutritionSummary { get; set; }

	/// <summary>
	/// Gets or sets the planning session info
	/// </summary>
	public AgentIterationInfo? IterationInfo { get; set; }
}
