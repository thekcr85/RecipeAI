using RecipeAI.Domain.Entities;
using RecipeAI.Domain.Enums;
using RecipeAI.Domain.Interfaces;
using RecipeAI.Infrastructure.AI.Services;

namespace RecipeAI.Infrastructure.Services;

/// <summary>
/// Implementation of meal planning service using AI agent orchestration
/// </summary>
/// <param name="orchestrator">AI agent orchestrator</param>
public class MealPlanningDomainService(AgentOrchestrator orchestrator) : IMealPlanningService
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
	public async Task<MealPlan> GenerateMealPlanAsync(
		DietType dietType,
		int numberOfDays,
		int targetCalories,
		decimal budgetLimit,
		CancellationToken cancellationToken = default)
	{
		return await orchestrator.CreateMealPlanAsync(
			dietType,
			numberOfDays,
			targetCalories,
			budgetLimit,
			cancellationToken);
	}
}
