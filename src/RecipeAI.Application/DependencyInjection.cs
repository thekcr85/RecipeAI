using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RecipeAI.Application.Services;

namespace RecipeAI.Application;

/// <summary>
/// Dependency injection configuration for Application layer
/// </summary>
public static class DependencyInjection
{
	/// <summary>
	/// Registers Application layer services
	/// </summary>
	/// <param name="services">Service collection</param>
	/// <returns>Service collection for chaining</returns>
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddScoped<MealPlanningService>();
		services.AddValidatorsFromAssemblyContaining<MealPlanningService>();

		return services;
	}
}
