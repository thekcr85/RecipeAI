using RecipeAI.Domain.Entities;
using RecipeAI.Domain.Enums;

namespace RecipeAI.Domain.Interfaces;

/// <summary>
/// Repository interface for meal plan operations
/// </summary>
public interface IMealPlanRepository
{
	/// <summary>
	/// Gets a meal plan by its identifier including all related data
	/// </summary>
	/// <param name="id">The meal plan identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The meal plan if found, otherwise null</returns>
	Task<MealPlan?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets all meal plans
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Collection of all meal plans</returns>
	Task<IEnumerable<MealPlan>> GetAllAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Gets meal plans by diet type
	/// </summary>
	/// <param name="dietType">The diet type to filter by</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>Collection of meal plans matching the diet type</returns>
	Task<IEnumerable<MealPlan>> GetByDietTypeAsync(DietType dietType, CancellationToken cancellationToken = default);

	/// <summary>
	/// Adds a new meal plan
	/// </summary>
	/// <param name="mealPlan">The meal plan to add</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The added meal plan with generated identifier</returns>
	Task<MealPlan> AddAsync(MealPlan mealPlan, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a meal plan
	/// </summary>
	/// <param name="id">The meal plan identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}