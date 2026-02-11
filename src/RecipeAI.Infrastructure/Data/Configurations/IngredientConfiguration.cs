using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeAI.Domain.Entities;

namespace RecipeAI.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for Ingredient
/// </summary>
public class IngredientConfiguration : IEntityTypeConfiguration<Ingredient>
{
	/// <summary>
	/// Configures the Ingredient entity
	/// </summary>
	/// <param name="builder">Entity type builder</param>
	public void Configure(EntityTypeBuilder<Ingredient> builder)
	{
		builder.ToTable("Ingredients");

		builder.HasKey(i => i.Id);

		builder.Property(i => i.Name)
			.IsRequired()
			.HasMaxLength(100);

		builder.Property(i => i.Unit)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(i => i.Quantity)
			.HasPrecision(8, 2);

		builder.Property(i => i.Cost)
			.HasPrecision(8, 2);

		builder.HasIndex(i => i.Name);
	}
}