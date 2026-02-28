using RecipeAI.Domain.Entities;

namespace RecipeAI.Domain.Interfaces;

public interface IPlanningSessionRepository
{
	Task<PlanningSession> AddAsync(PlanningSession session, CancellationToken cancellationToken = default);
	Task SaveChangesAsync(CancellationToken cancellationToken = default);
}