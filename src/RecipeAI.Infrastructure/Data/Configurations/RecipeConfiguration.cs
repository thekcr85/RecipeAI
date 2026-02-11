using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeAI.Domain.Entities;

namespace RecipeAI.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Recipe
/// </summary>
public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
	/// <summary>
	/// Configures the Recipe entity
	/// </summary>
	/// <param name="builder">Entity type builder</param>
	public void Configure(EntityTypeBuilder<Recipe> builder)
	{
		builder.ToTable("Recipes");

		builder.HasKey(r => r.Id);

		builder.Property(r => r.Name)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(r => r.MealType)
			.IsRequired()
			.HasConversion<string>();

		builder.Property(r => r.Instructions)
			.IsRequired()
			.HasMaxLength(2000);

		builder.Property(r => r.ProteinGrams)
			.HasPrecision(6, 2);

		builder.Property(r => r.CarbsGrams)
			.HasPrecision(6, 2);

		builder.Property(r => r.FatGrams)
			.HasPrecision(6, 2);

		builder.Property(r => r.EstimatedCost)
			.HasPrecision(8, 2);

		builder.HasMany(r => r.Ingredients)
			.WithOne(i => i.Recipe)
			.HasForeignKey(i => i.RecipeId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasIndex(r => r.MealType);
		builder.HasIndex(r => r.DayNumber);
	}
}