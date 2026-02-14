using Microsoft.Extensions.Options;
using OpenAI.Chat;
using RecipeAI.Infrastructure.AI.Options;
using RecipeAI.Infrastructure.AI.Prompts;

namespace RecipeAI.Infrastructure.AI.Agents;

/// <summary>
/// AI Agent responsible for budget optimization
/// </summary>
public class BudgetOptimizerAgent : BaseAgent
{
	public BudgetOptimizerAgent(IOptions<OpenAIOptions> options) : base(options)
	{
	}

	protected override string SystemPrompt => SystemPrompts.BudgetOptimizerAgent;
	protected override string AgentName => "BudgetOptimizer";

	/// <summary>
	/// Optimizes the meal plan to stay within budget
	/// </summary>
	public async Task<string> OptimizeBudgetAsync(
		string mealPlanJson,
		decimal budgetLimit,
		CancellationToken cancellationToken = default)
	{
		var chatClient = Client.GetChatClient(Options.Model);

		var userPrompt = $@"
Budget limit: {budgetLimit} PLN

Meal plan to evaluate:
{mealPlanJson}

Please evaluate this meal plan's cost and respond in the required JSON format.
";

		var messages = new List<ChatMessage>
		{
			new SystemChatMessage(SystemPrompt),
			new UserChatMessage(userPrompt)
		};

		var chatOptions = new ChatCompletionOptions
		{
			MaxOutputTokenCount = Options.MaxTokens,
			Temperature = (float)Options.Temperature
		};

		var response = await chatClient.CompleteChatAsync(messages, chatOptions, cancellationToken);

		return response.Value.Content[0].Text;
	}
}
