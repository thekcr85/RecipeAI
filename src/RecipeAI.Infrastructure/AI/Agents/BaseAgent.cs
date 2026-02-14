using Microsoft.Agents.AI.OpenAI;
using Microsoft.Extensions.Options;
using OpenAI;
using RecipeAI.Infrastructure.AI.Options;

namespace RecipeAI.Infrastructure.AI.Agents;

/// <summary>
/// Base class for all AI agents using Microsoft Agent Framework
/// </summary>
public abstract class BaseAgent
{
	protected readonly OpenAIClient Client;
	protected readonly OpenAIOptions Options;

	protected BaseAgent(IOptions<OpenAIOptions> options)
	{
		Options = options.Value;
		Client = new OpenAIClient(Options.ApiKey);
	}

	/// <summary>
	/// Gets the system prompt for this agent
	/// </summary>
	protected abstract string SystemPrompt { get; }

	/// <summary>
	/// Gets the agent's name
	/// </summary>
	protected abstract string AgentName { get; }
}
