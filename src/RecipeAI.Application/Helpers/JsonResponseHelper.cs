using System.Text.Json;
using System.Text.RegularExpressions;

namespace RecipeAI.Application.Helpers;

public static class JsonResponseHelper
{
	public static string CleanJsonResponse(string response)
	{
		if (string.IsNullOrWhiteSpace(response))
			return string.Empty;

		// Remove markdown code blocks if present
		var cleaned = Regex.Replace(response, @"```json\s*", "");
		cleaned = Regex.Replace(cleaned, @"```\s*", "");

		// Find first { and last } to extract pure JSON
		var start = cleaned.IndexOf('{');
		var end = cleaned.LastIndexOf('}');

		if (start == -1 || end == -1 || end <= start)
			return cleaned.Trim();

		return cleaned[start..(end + 1)].Trim();
	}

	public static bool IsValidJson(string json)
	{
		if (string.IsNullOrWhiteSpace(json))
			return false;

		try
		{
			JsonDocument.Parse(json);
			return true;
		}
		catch
		{
			return false;
		}
	}
}