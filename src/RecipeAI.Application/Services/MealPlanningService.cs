using RecipeAI.Application.DTOs;
using RecipeAI.Application.Exceptions;
using RecipeAI.Application.Mappings;
using RecipeAI.Application.Validators;
using RecipeAI.Domain.Entities;
using RecipeAI.Domain.Interfaces;

namespace RecipeAI.Application.Services;

/// <summary>
/// Service for meal planning operations
/// </summary>
/// <param name="mealPlanningDomain">Domain meal planning service</param>
/// <param name="mealPlanRepository">Meal plan repository</param>
/// <param name="recipeRepository">Recipe repository</param>
public class MealPlanningService(
	IMealPlanningService mealPlanningDomain,
	IMealPlanRepository mealPlanRepository,
	IRecipeRepository recipeRepository)
{
	/// <summary>
	/// Generates a new meal plan using AI
	/// </summary>
	/// <param name="request">The meal plan request</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The generated meal plan response</returns>
	/// <exception cref="MealPlanningException">Thrown when validation fails or generation fails</exception>
	public async Task<MealPlanResponse> GenerateMealPlanAsync(
		MealPlanRequest request,
		CancellationToken cancellationToken = default)
	{
		var validator = new MealPlanRequestValidator();
		var validationResult = await validator.ValidateAsync(request, cancellationToken);

		if (!validationResult.IsValid)
		{
			var errors = string.Join(", ", validationResult.Errors.Select(e => e.ErrorMessage));
			throw new MealPlanningException($"Walidacja nie powiodła się: {errors}");
		}

		try
		{
			var mealPlan = await mealPlanningDomain.GenerateMealPlanAsync(
				request.DietType,
				request.NumberOfDays,
				request.TargetCalories,
				request.BudgetLimit,
				cancellationToken);

			return mealPlan.ToResponse();
		}
		catch (Exception ex)
		{
			throw new MealPlanningException("Nie udało się wygenerować planu posiłków", ex);
		}
	}

	/// <summary>
	/// Gets a meal plan by identifier
	/// </summary>
	/// <param name="id">The meal plan identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The meal plan response or null if not found</returns>
	public async Task<MealPlanResponse?> GetMealPlanByIdAsync(
		int id,
		CancellationToken cancellationToken = default)
	{
		var mealPlan = await mealPlanRepository.GetByIdAsync(id, cancellationToken);
		return mealPlan?.ToResponse();
	}

	/// <summary>
	/// Gets all meal plans
	/// </summary>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of meal plan responses</returns>
	public async Task<List<MealPlanResponse>> GetAllMealPlansAsync(
		CancellationToken cancellationToken = default)
	{
		var mealPlans = await mealPlanRepository.GetAllAsync(cancellationToken);
		return mealPlans.Select(mp => mp.ToResponse()).ToList();
	}

	/// <summary>
	/// Deletes a meal plan
	/// </summary>
	/// <param name="id">The meal plan identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	public async Task DeleteMealPlanAsync(
		int id,
		CancellationToken cancellationToken = default)
	{
		await mealPlanRepository.DeleteAsync(id, cancellationToken);
	}

	/// <summary>
	/// Gets recipes by meal plan identifier
	/// </summary>
	/// <param name="mealPlanId">The meal plan identifier</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>List of recipe DTOs</returns>
	public async Task<List<RecipeDto>> GetRecipesByMealPlanAsync(
		int mealPlanId,
		CancellationToken cancellationToken = default)
	{
		var recipes = await recipeRepository.GetByMealPlanIdAsync(mealPlanId, cancellationToken);
		return recipes.Select(r => r.ToDto()).ToList();
	}
}
