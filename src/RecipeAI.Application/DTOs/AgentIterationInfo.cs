using RecipeAI.Domain.Enums;

namespace RecipeAI.Application.DTOs;

/// <summary>
/// Information about AI agent iterations during planning
/// </summary>
public class AgentIterationInfo
{
	/// <summary>
	/// Gets or sets the planning session identifier
	/// </summary>
	public int SessionId { get; set; }

	/// <summary>
	/// Gets or sets the session status
	/// </summary>
	public SessionStatus Status { get; set; }

	/// <summary>
	/// Gets or sets the number of iterations
	/// </summary>
	public int IterationCount { get; set; }

	/// <summary>
	/// Gets or sets when the session started
	/// </summary>
	public DateTime StartedAt { get; set; }

	/// <summary>
	/// Gets or sets when the session completed
	/// </summary>
	public DateTime? CompletedAt { get; set; }

	/// <summary>
	/// Gets or sets the iteration logs
	/// </summary>
	public List<string> IterationLogs { get; set; } = [];
}
