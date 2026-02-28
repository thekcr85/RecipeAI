using RecipeAI.Domain.Entities;
using RecipeAI.Domain.Enums;

namespace RecipeAI.Application.Interfaces;

public interface IAgentOrchestrator
{
	Task<MealPlan> CreateMealPlanAsync(
		DietType dietType,
		int numberOfDays,
		int targetCalories,
		decimal budgetLimit,
		CancellationToken cancellationToken = default);
}