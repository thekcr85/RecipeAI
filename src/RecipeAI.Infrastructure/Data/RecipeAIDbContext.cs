using Microsoft.EntityFrameworkCore;
using RecipeAI.Domain.Entities;

namespace RecipeAI.Infrastructure.Data;

/// <summary>
/// Database context for RecipeAI application
/// </summary>
/// <param name="options">Database context options</param>
public class RecipeAIDbContext(DbContextOptions<RecipeAIDbContext> options) : DbContext(options)
{
	/// <summary>
	/// Gets or sets the meal plans collection
	/// </summary>
	public DbSet<MealPlan> MealPlans => Set<MealPlan>();

	/// <summary>
	/// Gets or sets the recipes collection
	/// </summary>
	public DbSet<Recipe> Recipes => Set<Recipe>();

	/// <summary>
	/// Gets or sets the ingredients collection
	/// </summary>
	public DbSet<Ingredient> Ingredients => Set<Ingredient>();

	/// <summary>
	/// Gets or sets the planning sessions collection
	/// </summary>
	public DbSet<PlanningSession> PlanningSessions => Set<PlanningSession>();

	/// <summary>
	/// Configures the database model
	/// </summary>
	/// <param name="modelBuilder">Model builder instance</param>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		modelBuilder.ApplyConfigurationsFromAssembly(typeof(RecipeAIDbContext).Assembly);
	}
}