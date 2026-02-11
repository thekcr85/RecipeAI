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
		// Ensure database is created
		await context.Database.EnsureCreatedAsync(cancellationToken);

		// Check if already seeded
		if (await context.Recipes.AnyAsync(cancellationToken))
		{
			return; // Database already seeded
		}

		await SeedSampleRecipesAsync(context, cancellationToken);
	}

	/// <summary>
	/// Seeds 10 sample recipes for demonstration
	/// </summary>
	private static async Task SeedSampleRecipesAsync(RecipeAIDbContext context, CancellationToken cancellationToken)
	{
		var sampleRecipes = new List<Recipe>
		{
			// Recipe 1
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
			// Recipe 2
			new()
			{
				Name = "Jajecznica z warzywami",
				MealType = MealType.Breakfast,
				DayNumber = 1,
				Instructions = "Usmaż warzywa, dodaj jajka i wymieszaj do uzyskania puszystej jajecznicy.",
				PreparationTimeMinutes = 15,
				Calories = 280,
				ProteinGrams = 18,
				CarbsGrams = 12,
				FatGrams = 18,
				EstimatedCost = 7.00m,
				Ingredients =
				[
					new() { Name = "Jajka", Quantity = 3, Unit = "szt", Cost = 3.00m },
					new() { Name = "Pomidor", Quantity = 100, Unit = "g", Cost = 1.50m },
					new() { Name = "Cebula", Quantity = 50, Unit = "g", Cost = 0.50m },
					new() { Name = "Masło", Quantity = 10, Unit = "g", Cost = 2.00m }
				]
			},
			// Recipe 3
			new()
			{
				Name = "Sałatka grecka",
				MealType = MealType.Lunch,
				DayNumber = 1,
				Instructions = "Pokrój warzywa, dodaj ser feta, oliwki i polej oliwą z oliwek.",
				PreparationTimeMinutes = 10,
				Calories = 220,
				ProteinGrams = 8,
				CarbsGrams = 15,
				FatGrams = 15,
				EstimatedCost = 12.00m,
				Ingredients =
				[
					new() { Name = "Ogórek", Quantity = 150, Unit = "g", Cost = 2.00m },
					new() { Name = "Pomidory", Quantity = 200, Unit = "g", Cost = 3.00m },
					new() { Name = "Ser feta", Quantity = 100, Unit = "g", Cost = 5.00m },
					new() { Name = "Oliwki", Quantity = 50, Unit = "g", Cost = 2.00m }
				]
			},
			// Recipe 4
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
			},
			// Recipe 5
			new()
			{
				Name = "Makaron carbonara",
				MealType = MealType.Dinner,
				DayNumber = 2,
				Instructions = "Ugotuj makaron. Zrób sos z jajek, sera i boczku.",
				PreparationTimeMinutes = 25,
				Calories = 650,
				ProteinGrams = 28,
				CarbsGrams = 72,
				FatGrams = 28,
				EstimatedCost = 15.00m,
				Ingredients =
				[
					new() { Name = "Makaron spaghetti", Quantity = 200, Unit = "g", Cost = 3.00m },
					new() { Name = "Boczek", Quantity = 100, Unit = "g", Cost = 6.00m },
					new() { Name = "Jajka", Quantity = 2, Unit = "szt", Cost = 2.00m },
					new() { Name = "Parmezan", Quantity = 50, Unit = "g", Cost = 4.00m }
				]
			},
			// Recipe 6
			new()
			{
				Name = "Smoothie owocowe",
				MealType = MealType.Snack,
				DayNumber = 2,
				Instructions = "Zmiksuj wszystkie składniki na gładką masę.",
				PreparationTimeMinutes = 5,
				Calories = 180,
				ProteinGrams = 6,
				CarbsGrams = 38,
				FatGrams = 2,
				EstimatedCost = 9.00m,
				Ingredients =
				[
					new() { Name = "Banan", Quantity = 1, Unit = "szt", Cost = 2.00m },
					new() { Name = "Truskawki", Quantity = 100, Unit = "g", Cost = 4.00m },
					new() { Name = "Jogurt naturalny", Quantity = 150, Unit = "ml", Cost = 3.00m }
				]
			},
			// Recipe 7
			new()
			{
				Name = "Zupa pomidorowa",
				MealType = MealType.Lunch,
				DayNumber = 2,
				Instructions = "Ugotuj pomidory z warzywami, zmiksuj i dodaj śmietanę.",
				PreparationTimeMinutes = 35,
				Calories = 280,
				ProteinGrams = 8,
				CarbsGrams = 32,
				FatGrams = 14,
				EstimatedCost = 12.00m,
				Ingredients =
				[
					new() { Name = "Pomidory", Quantity = 400, Unit = "g", Cost = 5.00m },
					new() { Name = "Marchew", Quantity = 100, Unit = "g", Cost = 1.00m },
					new() { Name = "Cebula", Quantity = 80, Unit = "g", Cost = 1.00m },
					new() { Name = "Śmietana", Quantity = 100, Unit = "ml", Cost = 5.00m }
				]
			},
			// Recipe 8
			new()
			{
				Name = "Pierś z indyka z ziemniakami",
				MealType = MealType.Dinner,
				DayNumber = 3,
				Instructions = "Upiecz indyka w piekarniku. Ugotuj ziemniaki i podawaj z warzywami.",
				PreparationTimeMinutes = 45,
				Calories = 480,
				ProteinGrams = 45,
				CarbsGrams = 48,
				FatGrams = 10,
				EstimatedCost = 22.00m,
				Ingredients =
				[
					new() { Name = "Pierś z indyka", Quantity = 250, Unit = "g", Cost = 15.00m },
					new() { Name = "Ziemniaki", Quantity = 200, Unit = "g", Cost = 2.00m },
					new() { Name = "Marchewka", Quantity = 100, Unit = "g", Cost = 1.00m },
					new() { Name = "Groszek", Quantity = 100, Unit = "g", Cost = 4.00m }
				]
			},
			// Recipe 9
			new()
			{
				Name = "Sałatka z tuńczykiem",
				MealType = MealType.Lunch,
				DayNumber = 3,
				Instructions = "Wymieszaj tuńczyka z warzywami i polej oliwą.",
				PreparationTimeMinutes = 12,
				Calories = 320,
				ProteinGrams = 28,
				CarbsGrams = 18,
				FatGrams = 16,
				EstimatedCost = 16.00m,
				Ingredients =
				[
					new() { Name = "Tuńczyk w puszce", Quantity = 150, Unit = "g", Cost = 8.00m },
					new() { Name = "Sałata", Quantity = 100, Unit = "g", Cost = 2.00m },
					new() { Name = "Pomidor", Quantity = 120, Unit = "g", Cost = 2.00m },
					new() { Name = "Oliwa z oliwek", Quantity = 20, Unit = "ml", Cost = 4.00m }
				]
			},
			// Recipe 10
			new()
			{
				Name = "Naleśniki z serem",
				MealType = MealType.Breakfast,
				DayNumber = 3,
				Instructions = "Usmaż naleśniki i nadziej serem. Podawaj z owocami.",
				PreparationTimeMinutes = 25,
				Calories = 420,
				ProteinGrams = 18,
				CarbsGrams = 52,
				FatGrams = 16,
				EstimatedCost = 11.00m,
				Ingredients =
				[
					new() { Name = "Mąka", Quantity = 150, Unit = "g", Cost = 1.00m },
					new() { Name = "Mleko", Quantity = 250, Unit = "ml", Cost = 3.00m },
					new() { Name = "Jajka", Quantity = 2, Unit = "szt", Cost = 2.00m },
					new() { Name = "Ser biały", Quantity = 200, Unit = "g", Cost = 5.00m }
				]
			}
		};

		await context.Recipes.AddRangeAsync(sampleRecipes, cancellationToken);
		await context.SaveChangesAsync(cancellationToken);
	}
}