namespace RecipeAI.Infrastructure.AI.Options;

/// <summary>
/// Configuration settings for OpenAI integration
/// </summary>
public class OpenAISettings
{
	/// <summary>
	/// Configuration section name
	/// </summary>
	public const string SectionName = "OpenAI";

	/// <summary>
	/// Gets or sets the OpenAI API key
	/// </summary>
	public required string ApiKey { get; set; }

	/// <summary>
	/// Gets or sets the model name to use
	/// </summary>
	public string Model { get; set; } = "gpt-4o";

	/// <summary>
	/// Gets or sets the maximum number of tokens for completions
	/// </summary>
	public int MaxTokens { get; set; } = 2000;

	/// <summary>
	/// Gets or sets the temperature for response generation
	/// </summary>
	public double Temperature { get; set; } = 0.7;

	/// <summary>
	/// Gets or sets the maximum number of agent iterations
	/// </summary>
	public int MaxIterations { get; set; } = 3;
}