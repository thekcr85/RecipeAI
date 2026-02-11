using Microsoft.EntityFrameworkCore;
using RecipeAI.Domain.Entities;
using RecipeAI.Domain.Enums;

namespace RecipeAI.Infrastructure.Data;

/// <summary>
/// Database initializer with seed data
/// </summary>
public static class DbInitializer
{
	/// <summary>
	/// Initializes the database with seed data
	/// </summary>
	/// <param name="context">Database context</param>
	/// <param name="cancellationToken">Cancellation token</param>
	public static async Task InitializeAsync(RecipeAIDbContext context, CancellationToken cancellationToken = default)
	{
		await context.Database.MigrateAsync(cancellationToken);

		if (await context.Recipes.AnyAsync(cancellationToken))
		{
			return; // Database already seeded
		}

		await SeedSampleRecipesAsync(context, cancellationToken);
	}

	/// <summary>
	/// Seeds sample recipes for demonstration
	/// </summary>
	private static async Task SeedSampleRecipesAsync(RecipeAIDbContext context, CancellationToken cancellationToken)
	{
		var sampleRecipes = new List<Recipe>
		{
			new()
			{
				Name = "Owsianka z owocami",
				MealType = MealType.Breakfast,
				DayNumber = 1,
				Instructions = "Ugotuj płatki owsiane na mleku. Dodaj owoce sezonowe i miód.",
				PreparationTimeMinutes = 10,
				Calories = 350,
				ProteinGrams = 12,
				CarbsGrams = 58,
				FatGrams = 8,
				EstimatedCost = 8.50m,
				Ingredients =
				[
					new() { Name = "Płatki owsiane", Quantity = 80, Unit = "g", Cost = 2.00m },
					new() { Name = "Mleko", Quantity = 200, Unit = "ml", Cost = 2.50m },
					new() { Name = "Banany", Quantity = 1, Unit = "szt", Cost = 2.00m },
					new() { Name = "Miód", Quantity = 15, Unit = "g", Cost = 2.00m }
				]
			},
			new()
			{
				Name = "Kurczak z ryżem i warzywami",
				MealType = MealType.Dinner,
				DayNumber = 1,
				Instructions = "Ugotuj ryż. Usmaż kurczaka z warzywami na patelni.",
				PreparationTimeMinutes = 30,
				Calories = 520,
				ProteinGrams = 42,
				CarbsGrams = 55,
				FatGrams = 12,
				EstimatedCost = 18.00m,
				Ingredients =
				[
					new() { Name = "Pierś z kurczaka", Quantity = 200, Unit = "g", Cost = 10.00m },
					new() { Name = "Ryż", Quantity = 100, Unit = "g", Cost = 2.00m },
					new() { Name = "Brokuły", Quantity = 150, Unit = "g", Cost = 3.00m },
					new() { Name = "Papryka", Quantity = 100, Unit = "g", Cost = 3.00m }
				]
			}
		};

		await context.Recipes.AddRangeAsync(sampleRecipes, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);
	}
}