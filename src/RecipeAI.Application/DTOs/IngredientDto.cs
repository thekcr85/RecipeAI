namespace RecipeAI.Application.DTOs;

/// <summary>
/// Data transfer object for an ingredient
/// </summary>
public class IngredientDto
{
	/// <summary>
	/// Gets or sets the ingredient identifier
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets the ingredient name
	/// </summary>
	public required string Name { get; set; }

	/// <summary>
	/// Gets or sets the quantity
	/// </summary>
	public decimal Quantity { get; set; }

	/// <summary>
	/// Gets or sets the unit of measurement
	/// </summary>
	public required string Unit { get; set; }

	/// <summary>
	/// Gets or sets the cost
	/// </summary>
	public decimal Cost { get; set; }
}
