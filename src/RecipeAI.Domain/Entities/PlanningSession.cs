using RecipeAI.Domain.Enums;

namespace RecipeAI.Domain.Entities;

/// <summary>
/// Represents a planning session with AI agent iterations
/// </summary>
public class PlanningSession
{
	/// <summary>
	/// Gets or sets the unique identifier
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Gets or sets when this session was started
	/// </summary>
	public DateTime StartedAt { get; set; } = DateTime.UtcNow;

	/// <summary>
	/// Gets or sets when this session was completed
	/// </summary>
	public DateTime? CompletedAt { get; set; }

	/// <summary>
	/// Gets or sets the current status of the session
	/// </summary>
	public SessionStatus Status { get; set; } = SessionStatus.InProgress;

	/// <summary>
	/// Gets or sets the number of iterations the agents performed
	/// </summary>
	public int IterationCount { get; set; }

	/// <summary>
	/// Gets or sets the user's original request
	/// </summary>
	public required string UserRequest { get; set; }

	/// <summary>
	/// Gets or sets the final meal plan created in this session
	/// </summary>
	public MealPlan? FinalMealPlan { get; set; }

	/// <summary>
	/// Gets or sets detailed logs of agent iterations (JSON format)
	/// </summary>
	public string? IterationLogs { get; set; }

	/// <summary>
	/// Marks the session as completed
	/// </summary>
	public void Complete()
	{
		CompletedAt = DateTime.UtcNow;
		Status = SessionStatus.Completed;
	}

	/// <summary>
	/// Marks the session as failed
	/// </summary>
	public void Fail()
	{
		CompletedAt = DateTime.UtcNow;
		Status = SessionStatus.Failed;
	}
}