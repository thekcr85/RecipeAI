using Microsoft.Agents.AI;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using RecipeAI.Application.Interfaces.Agents;
using RecipeAI.Infrastructure.AI.Options;
using RecipeAI.Infrastructure.AI.Prompts;

namespace RecipeAI.Infrastructure.AI.Agents;

/// <summary>
/// AI Agent responsible for validating nutritional balance
/// </summary>
public class NutritionCriticAgent : INutritionCriticAgent
{
	private readonly AIAgent _agent;

	public NutritionCriticAgent(IOptions<OpenAIOptions> options)
	{
		var opts = options.Value;
		_agent = new OpenAIClient(opts.ApiKey)
			.GetChatClient(opts.Model)
			.AsAIAgent(
				name: "NutritionCritic",
				instructions: SystemPrompts.NutritionCriticAgent);
	}

	/// <summary>
	/// Validates the nutritional balance and diet compliance of a meal plan
	/// </summary>
	public async Task<string> ValidateNutritionAsync(
		string mealPlanJson,
		int targetCalories,
		string dietType,
		CancellationToken cancellationToken = default)
	{
		var userPrompt = $"""
            Target daily calories: {targetCalories} kcal
            Diet type: {dietType}
            Meal plan to evaluate:
            {mealPlanJson}
            IMPORTANT: Check if ALL ingredients comply with {dietType} diet restrictions.
            Please evaluate this meal plan and respond in the required JSON format.
            """;

		var response = await _agent.RunAsync(userPrompt, cancellationToken: cancellationToken);
		return response.Text;
	}
}