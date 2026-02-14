using System.Text.Json;

namespace RecipeAI.Infrastructure.AI.Helpers;

/// <summary>
/// Helper methods for AI response processing
/// </summary>
public static class JsonResponseHelper
{
	/// <summary>
	/// Cleans AI response by removing markdown code blocks and attempts repair
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
			response = response[7..];
		}
		else if (response.StartsWith("```", StringComparison.OrdinalIgnoreCase))
		{
			response = response[3..];
		}

		// Remove ``` at end
		if (response.EndsWith("```"))
		{
			response = response[..^3];
		}

		response = response.Trim();

		// Attempt to repair incomplete JSON
		if (!IsValidJson(response))
		{
			response = RepairIncompleteJson(response);
		}

		return response;
	}

	/// <summary>
	/// Validates that the JSON is complete and well-formed
	/// </summary>
	public static bool IsValidJson(string json)
	{
		if (string.IsNullOrWhiteSpace(json))
			return false;

		try
		{
			using var doc = JsonDocument.Parse(json);
			return true;
		}
		catch (JsonException)
		{
			return false;
		}
	}

	/// <summary>
	/// Attempts to repair incomplete JSON by adding missing closing braces
	/// </summary>
	public static string RepairIncompleteJson(string json)
	{
		if (string.IsNullOrWhiteSpace(json))
			return json;

		// Count opening and closing braces
		int openBraces = json.Count(c => c == '{');
		int closeBraces = json.Count(c => c == '}');
		int openBrackets = json.Count(c => c == '[');
		int closeBrackets = json.Count(c => c == ']');

		// Add missing closing characters
		var repairedJson = json;
		
		// Add missing closing brackets first
		for (int i = 0; i < (openBrackets - closeBrackets); i++)
		{
			repairedJson += "]";
		}

		// Then add missing closing braces
		for (int i = 0; i < (openBraces - closeBraces); i++)
		{
			repairedJson += "}";
		}

		return repairedJson;
	}
}
