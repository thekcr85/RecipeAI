using Microsoft.Extensions.Options;
using OpenAI.Chat;
using RecipeAI.Infrastructure.AI.Options;
using RecipeAI.Infrastructure.AI.Prompts;

namespace RecipeAI.Infrastructure.AI.Agents;

/// <summary>
/// AI Agent responsible for validating nutritional balance
/// </summary>
public class NutritionCriticAgent : BaseAgent
{
	public NutritionCriticAgent(IOptions<OpenAIOptions> options) : base(options)
	{
	}

	protected override string SystemPrompt => SystemPrompts.NutritionCriticAgent;
	protected override string AgentName => "NutritionCritic";

	/// <summary>
	/// Validates the nutritional balance and diet compliance of a meal plan
	/// </summary>
	public async Task<string> ValidateNutritionAsync(
		string mealPlanJson,
		int targetCalories,
		string dietType,
		CancellationToken cancellationToken = default)
	{
		var chatClient = Client.GetChatClient(Options.Model);

		var userPrompt = $@"
Target daily calories: {targetCalories} kcal
Diet type: {dietType}

Meal plan to evaluate:
{mealPlanJson}

IMPORTANT: Check if ALL ingredients comply with {dietType} diet restrictions.
Please evaluate this meal plan and respond in the required JSON format.
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
