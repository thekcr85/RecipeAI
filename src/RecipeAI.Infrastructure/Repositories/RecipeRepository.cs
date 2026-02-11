using Microsoft.EntityFrameworkCore;
using RecipeAI.Domain.Entities;
using RecipeAI.Domain.Interfaces;
using RecipeAI.Infrastructure.Data;

namespace RecipeAI.Infrastructure.Repositories;

/// <summary>
/// Repository for recipe operations
/// </summary>
/// <param name="context">Database context</param>
public class RecipeRepository(RecipeAIDbContext context) : IRecipeRepository
{
	/// <inheritdoc />
	public async Task<Recipe?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		return await context.Recipes
			.Include(r => r.Ingredients)
			.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
	}

	/// <inheritdoc />
	public async Task<IEnumerable<Recipe>> GetByMealPlanIdAsync(int mealPlanId, CancellationToken cancellationToken = default)
	{
		return await context.Recipes
			.Include(r => r.Ingredients)
			.Where(r => r.MealPlanId == mealPlanId)
			.OrderBy(r => r.DayNumber)
			.ThenBy(r => r.MealType)
			.ToListAsync(cancellationToken);
	}
}