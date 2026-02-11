using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeAI.Domain.Entities;

namespace RecipeAI.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for MealPlan
/// </summary>
public class MealPlanConfiguration : IEntityTypeConfiguration<MealPlan>
{
	/// <summary>
	/// Configures the MealPlan entity
	/// </summary>
	/// <param name="builder">Entity type builder</param>
	public void Configure(EntityTypeBuilder<MealPlan> builder)
	{
		builder.ToTable("MealPlans");

		builder.HasKey(mp => mp.Id);

		builder.Property(mp => mp.Name)
			.IsRequired()
			.HasMaxLength(200);

		builder.Property(mp => mp.DietType)
			.IsRequired()
			.HasConversion<string>();

		builder.Property(mp => mp.TotalCost)
			.HasPrecision(10, 2);

		builder.Property(mp => mp.CreatedAt)
			.IsRequired();

		builder.HasMany(mp => mp.Recipes)
			.WithOne(r => r.MealPlan)
			.HasForeignKey(r => r.MealPlanId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasOne(mp => mp.PlanningSession)
			.WithOne(ps => ps.FinalMealPlan)
			.HasForeignKey<MealPlan>(mp => mp.PlanningSessionId)
			.OnDelete(DeleteBehavior.SetNull);
	}
}