namespace RecipeAI.Domain.Enums;

/// <summary>
/// Represents the status of a planning session
/// </summary>
public enum SessionStatus
{
	/// <summary>
	/// Session is currently being processed by agents
	/// </summary>
	InProgress = 0,

	/// <summary>
	/// Session completed successfully
	/// </summary>
	Completed = 1,

	/// <summary>
	/// Session failed due to an error
	/// </summary>
	Failed = 2,

	/// <summary>
	/// Session was cancelled by user
	/// </summary>
	Cancelled = 3
}