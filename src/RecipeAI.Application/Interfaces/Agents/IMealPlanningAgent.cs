using RecipeAI.Domain.Enums;

namespace RecipeAI.Application.Interfaces.Agents;

public interface IMealPlanningAgent
{
	Task<string> GenerateMealPlanAsync(
		DietType dietType,
		int numberOfDays,
		int targetCalories,
		decimal budgetLimit,
		CancellationToken cancellationToken = default);

	Task<string> RefineMealPlanAsync(
		string currentPlan,
		string feedback,
		CancellationToken cancellationToken = default);
}