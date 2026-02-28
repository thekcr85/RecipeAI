namespace RecipeAI.Application.Interfaces.Agents;

public interface IBudgetOptimizerAgent
{
	Task<string> OptimizeBudgetAsync(
		string mealPlanJson,
		decimal budgetLimit,
		CancellationToken cancellationToken = default);
}