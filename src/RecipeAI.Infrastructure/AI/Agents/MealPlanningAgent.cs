using Microsoft.Extensions.Options;
using OpenAI.Chat;
using RecipeAI.Domain.Enums;
using RecipeAI.Infrastructure.AI.Options;
using RecipeAI.Infrastructure.AI.Prompts;

namespace RecipeAI.Infrastructure.AI.Agents;

/// <summary>
/// AI Agent responsible for generating meal plans
/// </summary>
public class MealPlanningAgent : BaseAgent
{
	public MealPlanningAgent(IOptions<OpenAIOptions> options) : base(options)
	{
	}

	protected override string SystemPrompt => SystemPrompts.MealPlannerAgent;
	protected override string AgentName => "MealPlanner";

	/// <summary>
	/// Generates a meal plan based on user requirements
	/// </summary>
	public async Task<string> GenerateMealPlanAsync(
		DietType dietType,
		int numberOfDays,
		int targetCalories,
		decimal budgetLimit,
		CancellationToken cancellationToken = default)
	{
		var chatClient = Client.GetChatClient(Options.Model);

		var userPrompt = SystemPrompts.BuildPlannerRequest(dietType, numberOfDays, targetCalories, budgetLimit);

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

	/// <summary>
	/// Refines an existing meal plan based on feedback
	/// </summary>
	public async Task<string> RefineMealPlanAsync(
		string currentPlan,
		string feedback,
		CancellationToken cancellationToken = default)
	{
		var chatClient = Client.GetChatClient(Options.Model);

		var messages = new List<ChatMessage>
		{
			new SystemChatMessage(SystemPrompt),
			new UserChatMessage($"Current plan:\n{currentPlan}"),
			new UserChatMessage($"Feedback to address:\n{feedback}\n\nPlease generate an improved plan addressing this feedback.")
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
