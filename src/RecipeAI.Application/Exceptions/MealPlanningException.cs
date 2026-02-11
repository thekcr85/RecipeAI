namespace RecipeAI.Application.Exceptions;

/// <summary>
/// Exception thrown when meal planning operations fail
/// </summary>
public class MealPlanningException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MealPlanningException"/> class
	/// </summary>
	public MealPlanningException()
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="MealPlanningException"/> class with a message
	/// </summary>
	/// <param name="message">The error message</param>
	public MealPlanningException(string message) : base(message)
	{
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="MealPlanningException"/> class with a message and inner exception
	/// </summary>
	/// <param name="message">The error message</param>
	/// <param name="innerException">The inner exception</param>
	public MealPlanningException(string message, Exception innerException) : base(message, innerException)
	{
	}
}
