using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using RecipeAI.Domain.Enums;
using RecipeAI.Infrastructure.AI.Options;
using RecipeAI.Infrastructure.AI.Prompts;

namespace RecipeAI.Infrastructure.AI.Services;

/// <summary>
/// Service for meal planning agent using Microsoft Agent Framework
/// </summary>
public class MealPlanningAgentService(
	IChatClient chatClient,
	IOptions<OpenAISettings> settings)
{
	private readonly OpenAISettings _settings = settings.Value;

	/// <summary>
	/// Generates a meal plan using AI
	/// </summary>
	public async Task<string> GenerateMealPlanAsync(
		DietType dietType,
		int numberOfDays,
		int targetCalories,
		decimal budgetLimit,
		CancellationToken cancellationToken = default)
	{
		var messages = new List<ChatMessage>
		{
			new(ChatRole.System, SystemPrompts.MealPlannerAgent),
			new(ChatRole.User, SystemPrompts.BuildPlannerRequest(
				dietType, numberOfDays, targetCalories, budgetLimit))
		};

		var response = await chatClient.GetResponseAsync(messages, cancellationToken: cancellationToken);

		return response.Messages.LastOrDefault()?.Text ?? string.Empty;
	}

	/// <summary>
	/// Refines an existing meal plan based on feedback
	/// </summary>
	public async Task<string> RefineMealPlanAsync(
		string currentPlan,
		string feedback,
		CancellationToken cancellationToken = default)
	{
		var messages = new List<ChatMessage>
		{
			new(ChatRole.System, SystemPrompts.MealPlannerAgent),
			new(ChatRole.User, $"Aktualny plan:\n{currentPlan}\n\nOpinia:\n{feedback}\n\nPopraw plan zgodnie z opinią.")
		};

		var response = await chatClient.GetResponseAsync(messages, cancellationToken: cancellationToken);

		return response.Messages.LastOrDefault()?.Text ?? string.Empty;
	}
}