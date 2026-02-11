using Microsoft.Extensions.AI;
using RecipeAI.Infrastructure.AI.Prompts;

namespace RecipeAI.Infrastructure.AI.Services;

/// <summary>
/// Service for nutrition validation using AI
/// </summary>
/// <param name="chatClient">Chat client for AI communication</param>
public class NutritionCriticService(IChatClient chatClient)
{
	/// <summary>
	/// Validates nutritional quality of a meal plan
	/// </summary>
	public async Task<string> ValidateNutritionAsync(
		string mealPlanJson,
		int targetCalories,
		CancellationToken cancellationToken = default)
	{
		var messages = new List<ChatMessage>
		{
			new(ChatRole.System, SystemPrompts.NutritionCriticAgent),
			new(ChatRole.User,
				$"Plan posiłków:\n{mealPlanJson}\n\nCel kaloryczny: {targetCalories} kcal\n\nOceń wartości odżywcze.")
		};

		var response = await chatClient.GetResponseAsync(
			messages,
			cancellationToken: cancellationToken);

		return response.Messages.LastOrDefault()?.Text ?? string.Empty;
	}
}