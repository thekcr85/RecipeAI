using RecipeAI.Domain.Entities;
using RecipeAI.Domain.Interfaces;
using RecipeAI.Infrastructure.Data;

namespace RecipeAI.Infrastructure.Repositories;

public class PlanningSessionRepository(RecipeAIDbContext context) : IPlanningSessionRepository
{
	public async Task<PlanningSession> AddAsync(
		PlanningSession session,
		CancellationToken cancellationToken = default)
	{
		await context.PlanningSessions.AddAsync(session, cancellationToken);
		return session;
	}

	public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
		=> await context.SaveChangesAsync(cancellationToken);
}