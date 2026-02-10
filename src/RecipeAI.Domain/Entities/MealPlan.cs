using RecipeAI.Domain.Enums;

namespace RecipeAI.Domain.Entities;

/// <summary>
/// Represents a complete meal plan for a specified number of days
/// </summary>
public class MealPlan
{
	/// <summary>
	/// Gets or sets the unique identifier
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the name of the meal plan
	/// </summary>
	public required string Name { get; set; }

	/// <summary>
	/// Gets or sets the diet type for this meal plan
	/// </summary>
	public DietType DietType { get; set; }

	/// <summary>
	/// Gets or sets the number of days this plan covers
	/// </summary>
	public int NumberOfDays { get; set; }

	/// <summary>
	/// Gets or sets the daily calorie target
	/// </summary>
	public int TargetCalories { get; set; }

	/// <summary>
	/// Gets or sets the total estimated cost of all ingredients
	/// </summary>
	public decimal TotalCost { get; set; }

	/// <summary>
	/// Gets or sets when this plan was created
	/// </summary>
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets the collection of recipes in this meal plan
	/// </summary>
	public ICollection<Recipe> Recipes { get; set; } = [];

	/// <summary>
	/// Gets or sets the planning session that created this meal plan
	/// </summary>
	public PlanningSession? PlanningSession { get; set; }

	/// <summary>
	/// Gets or sets the planning session identifier
	/// </summary>
	public int? PlanningSessionId { get; set; }
}