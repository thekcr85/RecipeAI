namespace RecipeAI.Domain.Enums;

/// <summary>
/// Represents the type of diet for meal planning
/// </summary>
public enum DietType
{
	/// <summary>
	/// Standard balanced diet
	/// </summary>
	Standard = 0,

	/// <summary>
	/// Plant-based diet excluding all animal products
	/// </summary>
	Vegan = 1,

	/// <summary>
	/// Diet excluding meat but including dairy and eggs
	/// </summary>
	Vegetarian = 2,

	/// <summary>
	/// High-fat, low-carb ketogenic diet
	/// </summary>
	Keto = 3,

	/// <summary>
	/// Mediterranean diet rich in vegetables, fruits, and olive oil
	/// </summary>
	Mediterranean = 4,

	/// <summary>
	/// High-protein diet for muscle building
	/// </summary>
	HighProtein = 5,

	/// <summary>
	/// Low-carb diet
	/// </summary>
	LowCarb = 6
}