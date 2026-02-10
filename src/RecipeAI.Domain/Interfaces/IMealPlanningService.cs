using RecipeAI.Domain.Entities;
using RecipeAI.Domain.Enums;

namespace RecipeAI.Domain.Interfaces;

/// <summary>
/// Service interface for AI-powered meal planning operations
/// </summary>
public interface IMealPlanningService
{
	/// <summary>
	/// Generates a meal plan using AI agents with iterative refinement
	/// </summary>
	/// <param name="dietType">The desired diet type</param>
	/// <param name="numberOfDays">Number of days to plan for</param>
	/// <param name="targetCalories">Daily calorie target</param>
	/// <param name="budgetLimit">Maximum budget for the plan</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns>The generated meal plan with planning session details</returns>
	Task<MealPlan> GenerateMealPlanAsync(
		DietType dietType,
		int numberOfDays,
		int targetCalories,
		decimal budgetLimit,
		CancellationToken cancellationToken = default);
}