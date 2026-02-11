using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI;
using RecipeAI.Domain.Interfaces;
using RecipeAI.Infrastructure.AI.Options;
using RecipeAI.Infrastructure.AI.Services;
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

		// Domain Services
		services.AddScoped<IMealPlanningService, MealPlanningDomainService>();

		// OpenAI
		services.Configure<OpenAISettings>(configuration.GetSection(OpenAISettings.SectionName));

		var openAISettings = configuration
			.GetSection(OpenAISettings.SectionName)
			.Get<OpenAISettings>()
			?? throw new InvalidOperationException("OpenAI settings not configured");

		services.AddSingleton<IChatClient>(sp =>
			new OpenAIClient(openAISettings.ApiKey)
				.GetChatClient(openAISettings.Model)
				.AsIChatClient());

		// AI Services
		services.AddScoped<MealPlanningAgentService>();
		services.AddScoped<NutritionCriticService>();
		services.AddScoped<BudgetOptimizerService>();
		services.AddScoped<AgentOrchestrator>();

		return services;
	}
}