namespace RecipeAI.Application.Interfaces.Agents;

public interface INutritionCriticAgent
{
	Task<string> ValidateNutritionAsync(
		string mealPlanJson,
		int targetCalories,
		string dietType,
		CancellationToken cancellationToken = default);
}