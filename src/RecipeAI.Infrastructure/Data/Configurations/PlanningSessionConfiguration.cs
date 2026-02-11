using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecipeAI.Domain.Entities;

namespace RecipeAI.Infrastructure.Data.Configurations;

/// <summary>
/// Entity configuration for PlanningSession
/// </summary>
public class PlanningSessionConfiguration : IEntityTypeConfiguration<PlanningSession>
{
	/// <summary>
	/// Configures the PlanningSession entity
	/// </summary>
	/// <param name="builder">Entity type builder</param>
	public void Configure(EntityTypeBuilder<PlanningSession> builder)
	{
		builder.ToTable("PlanningSessions");

		builder.HasKey(ps => ps.Id);

		builder.Property(ps => ps.UserRequest)
			.IsRequired()
			.HasMaxLength(1000);

		builder.Property(ps => ps.Status)
			.IsRequired()
			.HasConversion<string>();

		builder.Property(ps => ps.StartedAt)
			.IsRequired();

		builder.Property(ps => ps.IterationLogs)
			.HasColumnType("json");

		builder.HasIndex(ps => ps.Status);
		builder.HasIndex(ps => ps.StartedAt);
	}
}