using FluentValidation;
using RecipeAI.Application.DTOs;

namespace RecipeAI.Application.Validators;

/// <summary>
/// Validator for meal plan requests
/// </summary>
public class MealPlanRequestValidator : AbstractValidator<MealPlanRequest>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MealPlanRequestValidator"/> class
	/// </summary>
	public MealPlanRequestValidator()
	{
		RuleFor(x => x.DietType)
			.IsInEnum()
			.WithMessage("Nieprawid³owy typ diety");

		RuleFor(x => x.NumberOfDays)
			.InclusiveBetween(1, 30)
			.WithMessage("Liczba dni musi byæ miêdzy 1 a 30");

		RuleFor(x => x.TargetCalories)
			.InclusiveBetween(1000, 5000)
			.WithMessage("Cel kaloryczny musi byæ miêdzy 1000 a 5000 kcal");

		RuleFor(x => x.BudgetLimit)
			.GreaterThan(0)
			.WithMessage("Bud¿et musi byæ wiêkszy ni¿ 0");
	}
}
