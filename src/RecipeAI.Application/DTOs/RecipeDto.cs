using RecipeAI.Domain.Enums;

namespace RecipeAI.Application.DTOs;

/// <summary>
/// Data transfer object for a recipe
/// </summary>
public class RecipeDto
{
	/// <summary>
	/// Gets or sets the recipe identifier
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the recipe name
	/// </summary>
	public required string Name { get; set; }

	/// <summary>
	/// Gets or sets the meal type
	/// </summary>
	public MealType MealType { get; set; }

	/// <summary>
	/// Gets or sets the day number
	/// </summary>
	public int DayNumber { get; set; }

	/// <summary>
	/// Gets or sets the instructions
	/// </summary>
	public required string Instructions { get; set; }

	/// <summary>
	/// Gets or sets the preparation time in minutes
	/// </summary>
	public int PreparationTimeMinutes { get; set; }

	/// <summary>
	/// Gets or sets the calories
	/// </summary>
	public int Calories { get; set; }

	/// <summary>
	/// Gets or sets the protein in grams
	/// </summary>
	public decimal ProteinGrams { get; set; }

	/// <summary>
	/// Gets or sets the carbohydrates in grams
	/// </summary>
	public decimal CarbsGrams { get; set; }

	/// <summary>
	/// Gets or sets the fat in grams
	/// </summary>
	public decimal FatGrams { get; set; }

	/// <summary>
	/// Gets or sets the estimated cost
	/// </summary>
	public decimal EstimatedCost { get; set; }

	/// <summary>
	/// Gets or sets the ingredients
	/// </summary>
	public List<IngredientDto> Ingredients { get; set; } = [];
}
