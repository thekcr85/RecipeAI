using Microsoft.Agents.AI;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using RecipeAI.Application.Interfaces.Agents;
using RecipeAI.Infrastructure.AI.Options;
using RecipeAI.Infrastructure.AI.Prompts;

namespace RecipeAI.Infrastructure.AI.Agents;

/// <summary>
/// AI Agent responsible for budget optimization
/// </summary>
public class BudgetOptimizerAgent : IBudgetOptimizerAgent
{
	private readonly AIAgent _agent;

	public BudgetOptimizerAgent(IOptions<OpenAIOptions> options)
	{
		var opts = options.Value;
		_agent = new OpenAIClient(opts.ApiKey)
			.GetChatClient(opts.Model)
			.AsAIAgent(
				name: "BudgetOptimizer",
				instructions: SystemPrompts.BudgetOptimizerAgent);
	}

	/// <summary>
	/// Optimizes the meal plan to stay within budget
	/// </summary>
	public async Task<string> OptimizeBudgetAsync(
		string mealPlanJson,
		decimal budgetLimit,
		CancellationToken cancellationToken = default)
	{
		var userPrompt = $"""
            Budget limit: {budgetLimit} PLN
            Meal plan to evaluate:
            {mealPlanJson}
            Please evaluate this meal plan's cost and respond in the required JSON format.
            """;

		var response = await _agent.RunAsync(userPrompt, cancellationToken: cancellationToken);
		return response.Text;
	}
}