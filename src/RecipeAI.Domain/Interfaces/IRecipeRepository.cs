using RecipeAI.Domain.Entities;

namespace RecipeAI.Domain.Interfaces;

/// <summary>
/// Repository interface for recipe operations
/// </summary>
public interface IRecipeRepository
{
	/// <summary>
	/// Gets a recipe by its identifier including ingredients
	/// </summary>
	/// <param name="id">The recipe identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The recipe if found, otherwise null</returns>
	Task<Recipe?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets all recipes for a specific meal plan
	/// </summary>
	/// <param name="mealPlanId">The meal plan identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Collection of recipes for the meal plan</returns>
	Task<IEnumerable<Recipe>> GetByMealPlanIdAsync(int mealPlanId, CancellationToken cancellationToken = default);
}