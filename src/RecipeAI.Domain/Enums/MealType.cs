namespace RecipeAI.Domain.Enums;

/// <summary>
/// Represents the type of meal within a day
/// </summary>
public enum MealType
{
	/// <summary>
	/// Morning meal
	/// </summary>
	Breakfast = 0,

	/// <summary>
	/// Midday meal
	/// </summary>
	Lunch = 1,

	/// <summary>
	/// Evening meal
	/// </summary>
	Dinner = 2,

	/// <summary>
	/// Small meal between main meals
	/// </summary>
	Snack = 3
}