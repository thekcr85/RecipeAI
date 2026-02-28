using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeAI.Application.Interfaces;
using RecipeAI.Application.Interfaces.Agents;
using RecipeAI.Application.Services;
using RecipeAI.Domain.Interfaces;
using RecipeAI.Infrastructure.AI.Agents;
using RecipeAI.Infrastructure.AI.Options;
using RecipeAI.Infrastructure.Data;
using RecipeAI.Infrastructure.Repositories;
using RecipeAI.Infrastructure.Services;

namespace RecipeAI.Infrastructure;

public static class DependencyInjection
{
	public static IServiceCollection AddInfrastructure(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		// Database
		var connectionString = configuration.GetConnectionString("DefaultConnection")
			?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");

		services.AddDbContext<RecipeAIDbContext>(options =>
			options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

		// Repositories
		services.AddScoped<IMealPlanRepository, MealPlanRepository>();
		services.AddScoped<IRecipeRepository, RecipeRepository>();
		services.AddScoped<IPlanningSessionRepository, PlanningSessionRepository>();

		// Domain Services
		services.AddScoped<IMealPlanningService, MealPlanningDomainService>();

		// OpenAI Configuration
		services.Configure<OpenAIOptions>(configuration.GetSection("OpenAI"));

		// AI Agents using Microsoft Agent Framework
		services.AddScoped<IMealPlanningAgent, MealPlanningAgent>();
		services.AddScoped<INutritionCriticAgent, NutritionCriticAgent>();
		services.AddScoped<IBudgetOptimizerAgent, BudgetOptimizerAgent>();

		// Agent Orchestrator
		services.AddScoped<IAgentOrchestrator, AgentOrchestrator>();

		return services;
	}
}
