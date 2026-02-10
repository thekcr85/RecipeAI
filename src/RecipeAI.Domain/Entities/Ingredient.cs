namespace RecipeAI.Domain.Entities;

/// <summary>
/// Represents an ingredient required for a recipe
/// </summary>
public class Ingredient
{
	/// <summary>
	/// Gets or sets the unique identifier
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the ingredient name
	/// </summary>
	public required string Name { get; set; }

	/// <summary>
	/// Gets or sets the quantity needed
	/// </summary>
	public decimal Quantity { get; set; }

	/// <summary>
	/// Gets or sets the unit of measurement (e.g., kg, g, ml, pcs)
	/// </summary>
	public required string Unit { get; set; }

	/// <summary>
	/// Gets or sets the estimated cost of this ingredient
	/// </summary>
	public decimal Cost { get; set; }

	/// <summary>
	/// Gets or sets the recipe this ingredient belongs to
	/// </summary>
	public Recipe? Recipe { get; set; }

	/// <summary>
	/// Gets or sets the recipe identifier
	/// </summary>
	public int? RecipeId { get; set; }
}