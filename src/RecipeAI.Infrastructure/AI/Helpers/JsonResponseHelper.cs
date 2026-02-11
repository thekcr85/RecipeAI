namespace RecipeAI.Infrastructure.AI.Helpers;

/// <summary>
/// Helper methods for AI response processing
/// </summary>
public static class JsonResponseHelper
{
	/// <summary>
	/// Cleans AI response by removing markdown code blocks
	/// </summary>
	/// <param name="response">Raw AI response</param>
	/// <returns>Clean JSON string</returns>
	public static string CleanJsonResponse(string response)
	{
		if (string.IsNullOrWhiteSpace(response))
			return response;

		// Remove markdown code blocks
		response = response.Trim();
		
		// Remove ```json at start
		if (response.StartsWith("```json", StringComparison.OrdinalIgnoreCase))
		{
			response = response[7..]; // Remove ```json
		}
		else if (response.StartsWith("```", StringComparison.OrdinalIgnoreCase))
		{
			response = response[3..]; // Remove ```
		}

		// Remove ``` at end
		if (response.EndsWith("```"))
		{
			response = response[..^3];
		}

		return response.Trim();
	}
}
