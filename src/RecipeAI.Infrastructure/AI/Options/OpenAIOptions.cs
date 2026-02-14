namespace RecipeAI.Infrastructure.AI.Options;

/// <summary>
/// Configuration options for OpenAI with Microsoft Agent Framework
/// </summary>
public class OpenAIOptions
{
	/// <summary>
	/// Gets or sets the OpenAI API key
	/// </summary>
	public required string ApiKey { get; set; }

	/// <summary>
	/// Gets or sets the model name to use (e.g., gpt-4o, gpt-4)
	/// </summary>
	public string Model { get; set; } = "gpt-4o";

	/// <summary>
	/// Gets or sets the maximum number of tokens for completions
	/// </summary>
	public int MaxTokens { get; set; } = 4000;

	/// <summary>
	/// Gets or sets the temperature for response generation (0.0 - 2.0)
	/// </summary>
	public decimal Temperature { get; set; } = 0.7m;

	/// <summary>
	/// Gets or sets the maximum number of refinement iterations
	/// </summary>
	public int MaxIterations { get; set; } = 3;
}
