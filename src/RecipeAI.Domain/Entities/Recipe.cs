using RecipeAI.Domain.Enums;

namespace RecipeAI.Domain.Entities;

/// <summary>
/// Represents a single recipe with nutritional information
/// </summary>
public class Recipe
{
	/// <summary>
	/// Gets or sets the unique identifier
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the recipe name
	/// </summary>
	public required string Name { get; set; }

	/// <summary>
	/// Gets or sets the meal type (breakfast, lunch, dinner, snack)
	/// </summary>
	public MealType MealType { get; set; }

	/// <summary>
	/// Gets or sets which day number this recipe is for
	/// </summary>
	public int DayNumber { get; set; }

	/// <summary>
	/// Gets or sets the cooking instructions
	/// </summary>
	public required string Instructions { get; set; }

	/// <summary>
	/// Gets or sets the preparation time in minutes
	/// </summary>
	public int PreparationTimeMinutes { get; set; }

	/// <summary>
	/// Gets or sets the calories per serving
	/// </summary>
	public int Calories { get; set; }

	/// <summary>
	/// Gets or sets the protein content in grams
	/// </summary>
	public decimal ProteinGrams { get; set; }

	/// <summary>
	/// Gets or sets the carbohydrates content in grams
	/// </summary>
	public decimal CarbsGrams { get; set; }

	/// <summary>
	/// Gets or sets the fat content in grams
	/// </summary>
	public decimal FatGrams { get; set; }

	/// <summary>
	/// Gets or sets the estimated cost for this recipe
	/// </summary>
	public decimal EstimatedCost { get; set; }

	/// <summary>
	/// Gets or sets the meal plan this recipe belongs to
	/// </summary>
	public MealPlan? MealPlan { get; set; }

	/// <summary>
	/// Gets or sets the meal plan identifier
	/// </summary>
	public int? MealPlanId { get; set; }

	/// <summary>
	/// Gets or sets the collection of ingredients for this recipe
	/// </summary>
	public ICollection<Ingredient> Ingredients { get; set; } = [];
}