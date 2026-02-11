using Microsoft.EntityFrameworkCore;
using RecipeAI.Domain.Entities;
using RecipeAI.Domain.Enums;
using RecipeAI.Domain.Interfaces;
using RecipeAI.Infrastructure.Data;

namespace RecipeAI.Infrastructure.Repositories;

/// <summary>
/// Repository for meal plan operations
/// </summary>
/// <param name="context">Database context</param>
public class MealPlanRepository(RecipeAIDbContext context) : IMealPlanRepository
{
	/// <inheritdoc />
	public async Task<MealPlan?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
	{
		return await context.MealPlans
			.Include(mp => mp.Recipes)
				.ThenInclude(r => r.Ingredients)
			.Include(mp => mp.PlanningSession)
			.FirstOrDefaultAsync(mp => mp.Id == id, cancellationToken);
	}

	/// <inheritdoc />
	public async Task<IEnumerable<MealPlan>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		return await context.MealPlans
			.Include(mp => mp.Recipes)
			.OrderByDescending(mp => mp.CreatedAt)
			.ToListAsync(cancellationToken);
	}

	/// <inheritdoc />
	public async Task<IEnumerable<MealPlan>> GetByDietTypeAsync(DietType dietType, CancellationToken cancellationToken = default)
	{
		return await context.MealPlans
			.Include(mp => mp.Recipes)
			.Where(mp => mp.DietType == dietType)
			.OrderByDescending(mp => mp.CreatedAt)
			.ToListAsync(cancellationToken);
	}

	/// <inheritdoc />
	public async Task<MealPlan> AddAsync(MealPlan mealPlan, CancellationToken cancellationToken = default)
	{
		await context.MealPlans.AddAsync(mealPlan, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);
		return mealPlan;
	}

	/// <inheritdoc />
	public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
	{
		var mealPlan = await context.MealPlans.FindAsync([id], cancellationToken);
		if (mealPlan is not null)
		{
			context.MealPlans.Remove(mealPlan);
			await context.SaveChangesAsync(cancellationToken);
		}
	}
}