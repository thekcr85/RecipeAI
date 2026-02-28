using Microsoft.Agents.AI;
using Microsoft.Extensions.Options;
using OpenAI;
using OpenAI.Chat;
using RecipeAI.Application.Interfaces.Agents;
using RecipeAI.Domain.Enums;
using RecipeAI.Infrastructure.AI.Options;
using RecipeAI.Infrastructure.AI.Prompts;

namespace RecipeAI.Infrastructure.AI.Agents;

/// <summary>
/// AI Agent responsible for generating meal plans
/// </summary>
public class MealPlanningAgent : IMealPlanningAgent
{
	private readonly AIAgent _agent;

	public MealPlanningAgent(IOptions<OpenAIOptions> options)
	{
		var opts = options.Value;
		_agent = new OpenAIClient(opts.ApiKey)
			.GetChatClient(opts.Model)
			.AsAIAgent(
				name: "MealPlanner",
				instructions: SystemPrompts.MealPlannerAgent);
	}

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
		var userPrompt = SystemPrompts.BuildPlannerRequest(dietType, numberOfDays, targetCalories, budgetLimit);
		var response = await _agent.RunAsync(userPrompt, cancellationToken: cancellationToken);
		return response.Text;
	}

	/// <summary>
	/// Refines an existing meal plan based on feedback
	/// </summary>
	public async Task<string> RefineMealPlanAsync(
		string currentPlan,
		string feedback,
		CancellationToken cancellationToken = default)
	{
		var userPrompt = $"""
            Current plan:
            {currentPlan}

            Feedback to address:
            {feedback}

            Please generate an improved plan addressing this feedback.
            """;

		var response = await _agent.RunAsync(userPrompt, cancellationToken: cancellationToken);
		return response.Text;
	}
}