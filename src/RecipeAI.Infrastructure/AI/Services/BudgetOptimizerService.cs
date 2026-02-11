using Microsoft.Extensions.AI;
using RecipeAI.Infrastructure.AI.Prompts;

namespace RecipeAI.Infrastructure.AI.Services;

/// <summary>
/// Service for budget optimization using AI
/// </summary>
/// <param name="chatClient">Chat client for AI communication</param>
public class BudgetOptimizerService(IChatClient chatClient)
{
	/// <summary>
	/// Optimizes meal plan costs
	/// </summary>
	public async Task<string> OptimizeBudgetAsync(
		string mealPlanJson,
		decimal budgetLimit,
		CancellationToken cancellationToken = default)
	{
		var messages = new List<ChatMessage>
		{
			new(ChatRole.System, SystemPrompts.BudgetOptimizerAgent),
			new(ChatRole.User,
				$"Plan posiłków:\n{mealPlanJson}\n\nLimit budżetu: {budgetLimit} PLN\n\nZoptymalizuj koszty.")
		};

		var response = await chatClient.GetResponseAsync(
			messages,
			cancellationToken: cancellationToken);

		return response.Messages.LastOrDefault()?.Text ?? string.Empty;
	}
}