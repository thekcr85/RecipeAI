using System.Text.Json;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RecipeAI.Domain.Entities;
using RecipeAI.Domain.Enums;
using RecipeAI.Infrastructure.AI.Options;
using RecipeAI.Infrastructure.AI.Prompts;

namespace RecipeAI.Infrastructure.AI.Services;

/// <summary>
/// Service for meal planning agent using Microsoft Agent Framework
/// </summary>
/// <param name="chatClient">Chat client for AI communication</param>
/// <param name="settings">OpenAI settings</param>
/// <param name="logger">Logger instance</param>
public class MealPlanningAgentService(
	IChatClient chatClient,
	IOptions<OpenAISettings> settings,
	ILogger<MealPlanningAgentService> logger)
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
		logger.LogInformation(
			"Generating meal plan: Diet={Diet}, Days={Days}, Calories={Calories}, Budget={Budget}",
			dietType, numberOfDays, targetCalories, budgetLimit);

		var messages = new List<ChatMessage>
		{
			new(ChatRole.System, SystemPrompts.MealPlannerAgent),
			new(ChatRole.User, SystemPrompts.BuildPlannerRequest(dietType, numberOfDays, targetCalories, budgetLimit))
		};

		var response = await chatClient.CompleteAsync(messages, cancellationToken: cancellationToken);

		var result = response.Message.Text ?? string.Empty;

		logger.LogInformation("Meal plan generated successfully");

		return result;
	}

	/// <summary>
	/// Refines an existing meal plan based on feedback
	/// </summary>
	public async Task<string> RefineMealPlanAsync(
		string currentPlan,
		string feedback,
		CancellationToken cancellationToken = default)
	{
		logger.LogInformation("Refining meal plan based on feedback");

		var messages = new List<ChatMessage>
		{
			new(ChatRole.System, SystemPrompts.MealPlannerAgent),
			new(ChatRole.User, $"Aktualny plan:\n{currentPlan}\n\nOpinia:\n{feedback}\n\nPopraw plan zgodnie z opinią.")
		};

		var response = await chatClient.CompleteAsync(messages, cancellationToken: cancellationToken);

		return response.Message.Text ?? string.Empty;
	}
}