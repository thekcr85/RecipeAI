namespace RecipeAI.Application.DTOs;

/// <summary>
/// Summary of nutritional information for a meal plan
/// </summary>
public class NutritionSummary
{
	/// <summary>
	/// Gets or sets the total calories
	/// </summary>
	public int TotalCalories { get; set; }

	/// <summary>
	/// Gets or sets the average daily calories
	/// </summary>
	public int AverageDailyCalories { get; set; }

	/// <summary>
	/// Gets or sets the total protein in grams
	/// </summary>
	public decimal TotalProteinGrams { get; set; }

	/// <summary>
	/// Gets or sets the total carbohydrates in grams
	/// </summary>
	public decimal TotalCarbsGrams { get; set; }

	/// <summary>
	/// Gets or sets the total fat in grams
	/// </summary>
	public decimal TotalFatGrams { get; set; }

	/// <summary>
	/// Gets or sets the protein percentage of total calories
	/// </summary>
	public decimal ProteinPercentage { get; set; }

	/// <summary>
	/// Gets or sets the carbs percentage of total calories
	/// </summary>
	public decimal CarbsPercentage { get; set; }

	/// <summary>
	/// Gets or sets the fat percentage of total calories
	/// </summary>
	public decimal FatPercentage { get; set; }
}
