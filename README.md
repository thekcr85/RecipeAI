# RecipeAI 🍽️

> AI-powered meal planning with multi-agent refinement using OpenAI GPT-4o and Clean Architecture

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4)](https://blazor.net/)
[![OpenAI](https://img.shields.io/badge/OpenAI-GPT--4o-412991)](https://openai.com/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1)](https://www.mysql.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED)](https://www.docker.com/)

## Overview

A production-ready Blazor Server application demonstrating **Clean Architecture** principles with **Multi-Agent AI** collaboration for intelligent meal planning. Three specialized AI agents work together to generate, validate, and optimize personalized meal plans through iterative refinement.

### What It Does

- 🤖 **Planner Agent** generates meal plans strictly following diet type (Vegan, Keto, Mediterranean, etc.)
- 🔍 **Nutrition Critic Agent** validates macros, nutritional balance, AND diet compliance (catches violations like dairy in vegan diets)
- 💰 **Budget Optimizer Agent** keeps plans within budget and suggests diet-compliant alternatives
- 🔁 **Iterative Refinement** (up to 3 iterations) with graceful degradation
- 📊 Complete nutrition summaries with macro breakdown
- 💾 Plans persisted in MySQL with full iteration audit logs
- 🛡️ Automatic JSON validation and repair for incomplete AI responses
- 🏗️ **Proper Clean Architecture** with clear separation of concerns

### Key Features

- **Strict Diet Compliance**: Automatically rejects plans with diet violations (e.g., eggs in vegan diet)
- **Multi-Agent Quality Assurance**: Three specialized agents collaborate to ensure perfect plans
- **Graceful Error Handling**: If refinement fails, system uses previous valid version
- **Production-Ready Architecture**: Built with Microsoft Agent Framework (preview) and .NET 9
- **Clean Architecture Implementation**: Proper layer separation with dependency inversion

## Tech Stack

```
.NET 9 + C# 13
Blazor Server (Interactive)
Microsoft.Agents.AI.OpenAI (Preview)
OpenAI GPT-4o
Entity Framework Core 9 + MySQL 8
Docker + Docker Compose
Clean Architecture
FluentValidation
```

## Quick Start

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [OpenAI API Key](https://platform.openai.com/api-keys)

### Run with Docker

```bash
# 1. Clone the repository
git clone https://github.com/thekcr85/RecipeAI.git
cd RecipeAI

# 2. Create .env file with your API key
cp .env.example .env
# Edit .env and add your OpenAI API key

# 3. Start containers
docker compose up

# 4. Open browser
# → http://localhost:5000
```

That's it! The app will:
- Start MySQL 8.0 database container
- Create schema automatically (EnsureCreated)
- Seed 10 sample recipes
- Launch Blazor Server app on port 5000

## Architecture

### Clean Architecture Implementation

This project follows **Clean Architecture** principles with proper separation of concerns and dependency inversion:

```
┌─────────────────────────────────────────────────────────────┐
│                        RecipeAI.Web                          │
│                    (Blazor Server - UI)                      │
│  - Razor Components (Pages, Layout)                         │
│  - Dependency Injection Configuration                       │
│  - Entry Point (Program.cs)                                 │
└────────────────────────┬────────────────────────────────────┘
                         │ depends on ↓
┌────────────────────────┴────────────────────────────────────┐
│                  RecipeAI.Infrastructure                     │
│              (External Services & Data Access)               │
│  - AI Agents (MealPlanningAgent, NutritionCriticAgent,      │
│    BudgetOptimizerAgent)                                    │
│  - EF Core DbContext & Repositories                         │
│  - Domain Service Implementations                           │
│  - AI Prompts & Configuration                               │
└────────────────────────┬────────────────────────────────────┘
                         │ depends on ↓
┌────────────────────────┴────────────────────────────────────┐
│                   RecipeAI.Application                       │
│              (Business Logic & Orchestration)                │
│  - Agent Interfaces (IMealPlanningAgent, etc.)              │
│  - Agent Orchestrator (multi-agent coordination)            │
│  - DTOs, Validators, Mappings                               │
│  - JSON Response Helper                                     │
└────────────────────────┬────────────────────────────────────┘
                         │ depends on ↓
┌────────────────────────┴────────────────────────────────────┐
│                      RecipeAI.Domain                         │
│                   (Core Business Domain)                     │
│  - Entities (MealPlan, Recipe, Ingredient, PlanningSession) │
│  - Enums (DietType, MealType, SessionStatus)                │
│  - Domain Interfaces (IMealPlanningService, Repositories)   │
│  - No external dependencies                                 │
└─────────────────────────────────────────────────────────────┘
```

### Why Web Doesn't Reference Domain?

Following Clean Architecture principles:

1. **Dependency Inversion Principle**: Web layer depends on abstractions (interfaces in Application layer), not concrete implementations
2. **Separation of Concerns**: Web handles presentation, Application coordinates business logic, Domain contains core business rules
3. **Loose Coupling**: Changes in Domain don't directly affect Web layer
4. **Testability**: Each layer can be tested independently
5. **Mediator Pattern**: Application layer acts as mediator between Web and Domain

**Dependency Flow**: `Web → Infrastructure → Application → Domain`

### Layer Responsibilities

#### 🎯 Domain Layer (Core)
**Purpose**: Contains core business logic and domain rules  
**No dependencies on other layers**

- **Entities**: `MealPlan`, `Recipe`, `Ingredient`, `PlanningSession`
- **Enums**: `DietType`, `MealType`, `SessionStatus`
- **Domain Interfaces**: 
  - `IMealPlanningService` - Domain service contract
  - `IMealPlanRepository`, `IRecipeRepository`, `IPlanningSessionRepository` - Repository contracts

#### 💼 Application Layer (Business Logic)
**Purpose**: Orchestrates business workflows and coordinates between layers  
**Dependencies**: Domain only

- **Agent Orchestrator**: `AgentOrchestrator` - Coordinates multi-agent collaboration workflow
- **Agent Interfaces**: `IMealPlanningAgent`, `INutritionCriticAgent`, `IBudgetOptimizerAgent`
- **Orchestrator Interface**: `IAgentOrchestrator` - Orchestration contract
- **DTOs**: `MealPlanRequest`, `MealPlanResponse`, `RecipeDto`, etc.
- **Validators**: `MealPlanRequestValidator` (FluentValidation)
- **Helpers**: `JsonResponseHelper` - Cleans and validates AI JSON responses
- **Service**: `MealPlanningService` - Application-level meal planning coordination

#### 🔧 Infrastructure Layer (External Services)
**Purpose**: Implements external concerns (database, AI, third-party services)  
**Dependencies**: Application, Domain

- **AI Agents** (Microsoft.Agents.AI.OpenAI):
  - `MealPlanningAgent` - Generates and refines meal plans
  - `NutritionCriticAgent` - Validates nutrition and diet compliance
  - `BudgetOptimizerAgent` - Optimizes costs
- **System Prompts**: Detailed agent instructions in Polish
- **Data Access**: 
  - `RecipeAIDbContext` - EF Core context
  - `MealPlanRepository`, `RecipeRepository`, `PlanningSessionRepository`
- **Domain Service Implementation**: `MealPlanningDomainService` - Delegates to orchestrator
- **Configuration**: `OpenAIOptions` - AI settings

#### 🎨 Web Layer (Presentation)
**Purpose**: User interface and presentation logic  
**Dependencies**: Infrastructure, Application (through DI)

- **Blazor Components**:
  - `Home.razor` - Landing page
  - `CreatePlan.razor` - Meal plan generator form
  - `MealPlans.razor` - List all plans
  - `MealPlanDetails.razor` - Plan details view
- **Layout**: `MainLayout.razor`, `NavMenu.razor`
- **Program.cs**: Entry point, DI configuration, database initialization

## How Multi-Agent Refinement Works

The application uses a **Planner-Critic-Optimizer** pattern with three specialized AI agents coordinated by the `AgentOrchestrator`:

```
User Request: Diet=Vegan, Days=7, Calories=2000, Budget=500PLN
                              │
                              ↓
┌─────────────────────────────────────────────────────────────┐
│                    AgentOrchestrator                         │
│              (Application Layer Coordinator)                 │
└─────────────────────────────────────────────────────────────┘
                              │
                              ↓
┌─────────────────────────────────────────────────────────────┐
│ Iteration 1: Initial Generation                            │
├─────────────────────────────────────────────────────────────┤
│ 🤖 MealPlanningAgent:                                       │
│    Generates 21 recipes (7 days × 3 meals)                 │
│                                                              │
│ 🔍 NutritionCriticAgent:                                    │
│    ❌ "Found cow's milk in recipe 3 (vegan violation)"     │
│    ❌ "Found eggs in recipe 7 (vegan violation)"           │
│    ❌ "Protein too low: 15% (target: 15-30%)"              │
│                                                              │
│ 💰 BudgetOptimizerAgent:                                    │
│    ❌ "Total cost 580 PLN exceeds budget limit 500 PLN"    │
│    💡 "Suggestions: Replace quinoa with rice (-40 PLN)"    │
└─────────────────────────────────────────────────────────────┘
         ↓ Feedback: Remove dairy/eggs, increase protein, reduce costs
┌─────────────────────────────────────────────────────────────┐
│ Iteration 2: Refinement                                    │
├─────────────────────────────────────────────────────────────┤
│ 🤖 MealPlanningAgent:                                       │
│    Applies feedback: milk→soy milk, eggs→tofu scramble     │
│                                                              │
│ 🔍 NutritionCriticAgent:                                    │
│    ✅ "All ingredients vegan-compliant"                     │
│    ✅ "Macros balanced (P:22% C:53% F:25%)"                │
│                                                              │
│ 💰 BudgetOptimizerAgent:                                    │
│    ❌ "Still 520 PLN, suggest cheaper protein sources"     │
│    💡 "Replace almond milk (15 PLN) with oat milk (8 PLN)" │
└─────────────────────────────────────────────────────────────┘
         ↓ Feedback: Optimize costs while maintaining vegan compliance
┌─────────────────────────────────────────────────────────────┐
│ Iteration 3: Final Polish                                  │
├─────────────────────────────────────────────────────────────┤
│ 🤖 MealPlanningAgent:                                       │
│    Applies budget optimizations with vegan alternatives    │
│                                                              │
│ 🔍 NutritionCriticAgent:                                    │
│    ✅ "Approved - vegan compliant + nutritionally balanced" │
│                                                              │
│ 💰 BudgetOptimizerAgent:                                    │
│    ✅ "Within budget: 485 PLN (saved 95 PLN)"              │
└─────────────────────────────────────────────────────────────┘
         ↓
    💾 Save to Database
    ✅ MealPlan created with 3 iterations logged
```

### Agent Architecture

#### 🤖 Meal Planning Agent (`MealPlanningAgent`)
**Location**: `RecipeAI.Infrastructure/AI/Agents/MealPlanningAgent.cs`  
**Interface**: `IMealPlanningAgent` (Application layer)  
**Built with**: Microsoft.Agents.AI.OpenAI

**Responsibilities**:
- Generates initial meal plans with complete recipes and ingredients
- STRICTLY adheres to diet type restrictions (e.g., no dairy/eggs for vegan)
- Refines plans based on aggregated feedback from Critic and Optimizer
- Uses 4000 tokens to generate complete JSON responses
- Handles both initial generation and iterative refinement

**Methods**:
- `GenerateMealPlanAsync()` - Creates initial meal plan
- `RefineMealPlanAsync()` - Refines existing plan based on feedback

---

#### 🔍 Nutrition Critic Agent (`NutritionCriticAgent`)
**Location**: `RecipeAI.Infrastructure/AI/Agents/NutritionCriticAgent.cs`  
**Interface**: `INutritionCriticAgent` (Application layer)  
**Built with**: Microsoft.Agents.AI.OpenAI

**Responsibilities**:
- **PRIMARY**: Validates diet compliance (automatically blocks forbidden ingredients)
- Checks macronutrient distribution (protein, carbs, fats)
- Ensures daily calorie targets are met (±10% tolerance)
- Verifies meal variety and nutritional balance
- Returns structured feedback with `dietViolations` array

**Validation Criteria**:
- Diet compliance (most critical - auto-reject on violations)
- Protein: 15-30% of daily calories
- Carbs: 45-65% (or per diet requirements)
- Fats: 20-35% (or per diet requirements)
- Minimum 3 main meals per day

**Methods**:
- `ValidateNutritionAsync()` - Evaluates plan and returns JSON feedback

---

#### 💰 Budget Optimizer Agent (`BudgetOptimizerAgent`)
**Location**: `RecipeAI.Infrastructure/AI/Agents/BudgetOptimizerAgent.cs`  
**Interface**: `IBudgetOptimizerAgent` (Application layer)  
**Built with**: Microsoft.Agents.AI.OpenAI

**Responsibilities**:
- Checks total meal plan cost against budget limit
- Suggests cheaper ingredient alternatives (MUST respect diet type)
- Recommends seasonal ingredients and bulk-buy options
- Provides cost savings estimates
- Never compromises diet compliance for cost reduction

**Methods**:
- `OptimizeBudgetAsync()` - Evaluates budget and suggests optimizations

---

### Agent Orchestrator (`AgentOrchestrator`)
**Location**: `RecipeAI.Application/Services/AgentOrchestrator.cs`  
**Interface**: `IAgentOrchestrator` (Application layer)

**Responsibilities**:
- Coordinates all three AI agents in sequence
- Manages iterative refinement loop (max 3 iterations)
- Tracks `PlanningSession` with iteration logs
- Implements graceful degradation (uses previous valid plan on errors)
- Validates JSON responses and handles malformed AI outputs
- Parses final JSON into domain entities

**Workflow**:
1. Create `PlanningSession` (tracks iteration history)
2. Call `MealPlanningAgent` for initial generation
3. Validate JSON response with `JsonResponseHelper`
4. Loop (up to 3 iterations):
   - Call `NutritionCriticAgent` for validation
   - Call `BudgetOptimizerAgent` for cost check
   - If both approve → exit loop
   - If not approved → aggregate feedback and call `MealPlanningAgent.RefineMealPlanAsync()`
5. Parse final JSON to `MealPlan` entity
6. Save with iteration logs to database

### Domain Service Bridge

#### `MealPlanningDomainService`
**Location**: `RecipeAI.Infrastructure/Services/MealPlanningDomainService.cs`  
**Interface**: `IMealPlanningService` (Domain layer)

Acts as a bridge between the domain interface and the application orchestrator:
- Implements domain interface `IMealPlanningService`
- Delegates to `IAgentOrchestrator` (Application layer)
- Keeps Domain layer independent of Application logic

This design allows Domain to define "what" needs to happen (`IMealPlanningService`) while Application defines "how" it happens (`AgentOrchestrator`).

### Quality Safeguards

1. **Diet Violation Detection**: Critic agent automatically rejects plans with forbidden ingredients
2. **JSON Validation** (`JsonResponseHelper`): 
   - Strips markdown code blocks from AI responses
   - Extracts pure JSON from mixed responses
   - Validates JSON structure before parsing
3. **Graceful Degradation**: If refinement fails, uses previous valid version (no crashes)
4. **Iteration Logging**: Full audit trail of all agent decisions saved to `PlanningSession`
5. **Error Recovery**: Catches JSON parsing errors and provides helpful diagnostics

## Supported Diet Types

The application supports six diet types with strict compliance validation:

### 🌱 Vegan Diet
- **Restrictions**: No meat, fish, dairy, eggs, honey, or any animal-derived products
- **Allowed**: Vegetables, fruits, legumes, grains, nuts, seeds, plant-based alternatives
- **Examples**: Soy milk, oat milk, almond milk, tofu, tempeh, seitan
- **Critic Validation**: Blocks cow's milk, butter, eggs, cheese, honey, gelatin, whey
- **Real Example**: "Found cow's milk in Smoothie recipe" → Auto-reject

### 🥗 Vegetarian Diet
- **Restrictions**: No meat, poultry, fish, or seafood
- **Allowed**: Dairy products, eggs, all plant-based foods
- **Examples**: Milk, cheese, yogurt, eggs, butter, legumes, grains
- **Critic Validation**: Blocks meat, poultry, fish, seafood

### 🥑 Keto Diet
- **Macros**: Very low carb (<10% of calories), high fat (65-75%), moderate protein (20-30%)
- **Allowed**: Meat, fish, eggs, high-fat dairy, low-carb vegetables, nuts, healthy oils
- **Restricted**: Bread, rice, pasta, potatoes, sugar, most fruits
- **Critic Validation**: Enforces strict carb limits, validates fat percentage

### 🌊 Mediterranean Diet
- **Focus**: Heart-healthy with emphasis on olive oil, fish, vegetables, fruits, nuts, whole grains
- **Characteristics**: Limited red meat, moderate dairy, abundant seafood
- **Key Ingredients**: Olive oil, fish, legumes, whole grains, fresh vegetables
- **Benefits**: Omega-3 rich, anti-inflammatory

### 💪 High Protein Diet
- **Macros**: Protein >30% of daily calories
- **Sources**: Lean meat, fish, eggs, legumes, Greek yogurt, cottage cheese
- **Purpose**: Muscle building, weight management, satiety
- **Critic Validation**: Ensures minimum 30% protein ratio

### 🍞 Low Carb Diet
- **Macros**: Carbs <25% of daily calories
- **Focus**: Reduced grains, sugars, and starches
- **Allowed**: Proteins, healthy fats, non-starchy vegetables
- **Critic Validation**: Enforces carb limit, validates fat and protein balance

## Application Pages

| Route | Component | Description |
|-------|-----------|-------------|
| `/` | `Home.razor` | Landing page with app overview and features |
| `/create-plan` | `CreatePlan.razor` | AI meal plan generator form (interactive) |
| `/meal-plans` | `MealPlans.razor` | Browse all created meal plans |
| `/meal-plan/{id}` | `MealPlanDetails.razor` | View plan details with recipes, nutrition, and iteration logs |

### Features per Page

**Home Page**:
- App introduction
- Supported diet types overview
- Quick start guide

**Create Plan Page**:
- Interactive Blazor form with real-time validation
- Diet type selection (6 options)
- Number of days (1-14)
- Daily calorie target (1200-4000)
- Budget limit in PLN
- Real-time AI generation with progress indicator

**Meal Plans Page**:
- List view of all generated plans
- Filter by diet type
- Quick preview of calories, cost, and recipes count
- Navigation to detailed view

**Meal Plan Details Page**:
- Day-by-day recipe breakdown
- Complete ingredient lists with costs
- Nutritional summary (calories, protein, carbs, fats)
- Planning session details (iterations, timestamps)
- Full iteration audit log

## Configuration

### Environment Variables (.env)

```bash
OPENAI_API_KEY=sk-your-openai-api-key-here
OPENAI_MODEL=gpt-4o
OPENAI_MAX_TOKENS=4000
OPENAI_TEMPERATURE=0.7
MYSQL_ROOT_PASSWORD=RecipeAI2024!Strong
MYSQL_DATABASE=RecipeAI
```

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=db;Database=RecipeAI;User=root;Password=RecipeAI2024!Strong;"
  },
  "OpenAI": {
    "ApiKey": "sk-your-openai-api-key-here",
    "Model": "gpt-4o",
    "MaxTokens": 4000,
    "Temperature": 0.7
  }
}
```

**Note**: For local development without Docker, change `Server=db` to `Server=localhost`.

## Project Structure

```
RecipeAI/
├── src/
│   ├── RecipeAI.Domain/                      # 🎯 Core Business Domain
│   │   ├── Entities/
│   │   │   ├── MealPlan.cs                   # Aggregate root for meal plans
│   │   │   ├── Recipe.cs                     # Individual recipe entity
│   │   │   ├── Ingredient.cs                 # Recipe ingredient
│   │   │   └── PlanningSession.cs            # AI iteration tracking
│   │   ├── Enums/
│   │   │   ├── DietType.cs                   # Vegan, Keto, Mediterranean, etc.
│   │   │   ├── MealType.cs                   # Breakfast, Lunch, Dinner, Snack
│   │   │   └── SessionStatus.cs              # InProgress, Completed, Failed
│   │   └── Interfaces/
│   │       ├── IMealPlanningService.cs       # Domain service contract
│   │       ├── IMealPlanRepository.cs        # Repository contracts
│   │       ├── IRecipeRepository.cs
│   │       └── IPlanningSessionRepository.cs
│   │
│   ├── RecipeAI.Application/                 # 💼 Business Logic & Orchestration
│   │   ├── Services/
│   │   │   ├── AgentOrchestrator.cs          # ⭐ Multi-agent coordinator
│   │   │   └── MealPlanningService.cs        # Application service
│   │   ├── Interfaces/
│   │   │   ├── IAgentOrchestrator.cs         # Orchestrator contract
│   │   │   └── Agents/
│   │   │       ├── IMealPlanningAgent.cs     # Agent contracts
│   │   │       ├── INutritionCriticAgent.cs
│   │   │       └── IBudgetOptimizerAgent.cs
│   │   ├── DTOs/                             # Data Transfer Objects
│   │   │   ├── MealPlanRequest.cs
│   │   │   ├── MealPlanResponse.cs
│   │   │   ├── RecipeDto.cs
│   │   │   ├── IngredientDto.cs
│   │   │   ├── NutritionSummary.cs
│   │   │   └── AgentIterationInfo.cs
│   │   ├── Validators/
│   │   │   └── MealPlanRequestValidator.cs   # FluentValidation
│   │   ├── Mappings/
│   │   │   └── MappingExtensions.cs          # Entity ↔ DTO mapping
│   │   ├── Helpers/
│   │   │   └── JsonResponseHelper.cs         # ⭐ AI response cleaning
│   │   ├── Exceptions/
│   │   │   └── MealPlanningException.cs
│   │   └── DependencyInjection.cs
│   │
│   ├── RecipeAI.Infrastructure/              # 🔧 External Services & Data
│   │   ├── AI/
│   │   │   ├── Agents/                       # ⭐ AI Agent Implementations
│   │   │   │   ├── MealPlanningAgent.cs      # Microsoft.Agents.AI
│   │   │   │   ├── NutritionCriticAgent.cs   # Microsoft.Agents.AI
│   │   │   │   └── BudgetOptimizerAgent.cs   # Microsoft.Agents.AI
│   │   │   ├── Prompts/
│   │   │   │   └── SystemPrompts.cs          # Agent instructions (Polish)
│   │   │   └── Options/
│   │   │       └── OpenAIOptions.cs          # AI configuration
│   │   ├── Data/
│   │   │   ├── RecipeAIDbContext.cs          # EF Core context
│   │   │   ├── DbInitializer.cs              # Database seeding
│   │   │   └── Configurations/               # Entity configurations
│   │   │       ├── MealPlanConfiguration.cs
│   │   │       ├── RecipeConfiguration.cs
│   │   │       ├── IngredientConfiguration.cs
│   │   │       └── PlanningSessionConfiguration.cs
│   │   ├── Repositories/
│   │   │   ├── MealPlanRepository.cs
│   │   │   ├── RecipeRepository.cs
│   │   │   └── PlanningSessionRepository.cs  # ⭐ Iteration tracking
│   │   ├── Services/
│   │   │   └── MealPlanningDomainService.cs  # ⭐ Domain bridge
│   │   └── DependencyInjection.cs            # ⭐ DI registration
│   │
│   └── RecipeAI.Web/                         # 🎨 Blazor Server UI
│       ├── Components/
│       │   ├── Pages/
│       │   │   ├── Home.razor                # Landing page
│       │   │   ├── CreatePlan.razor          # Plan generator
│       │   │   ├── MealPlans.razor           # Plans list
│       │   │   ├── MealPlanDetails.razor     # Plan details
│       │   │   └── Error.razor               # Error handling
│       │   ├── Layout/
│       │   │   ├── MainLayout.razor          # Main layout
│       │   │   └── NavMenu.razor             # Navigation
│       │   ├── App.razor                     # Root component
│       │   ├── Routes.razor                  # Routing config
│       │   └── _Imports.razor                # Global usings
│       ├── wwwroot/                          # Static assets
│       ├── appsettings.json                  # Configuration
│       └── Program.cs                        # ⭐ Entry point & DI
│
├── docker-compose.yml                        # Docker orchestration
├── Dockerfile                                # Web app container
└── README.md                                 # This file
```

**⭐ Key Files** for understanding the implementation:
- `AgentOrchestrator.cs` - Multi-agent coordination workflow
- `JsonResponseHelper.cs` - AI response cleaning and validation
- `MealPlanningAgent.cs` - Plan generation agent
- `NutritionCriticAgent.cs` - Nutrition validation agent
- `BudgetOptimizerAgent.cs` - Budget optimization agent
- `MealPlanningDomainService.cs` - Domain-Application bridge
- `DependencyInjection.cs` - Agent DI registration by interface

## Local Development

### Without Docker

```bash
# 1. Start MySQL container
docker run -d -p 3306:3306 \
  -e MYSQL_ROOT_PASSWORD=RecipeAI2024!Strong \
  -e MYSQL_DATABASE=RecipeAI \
  --name recipeai-mysql \
  mysql:8.0

# 2. Update src/RecipeAI.Web/appsettings.json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=RecipeAI;User=root;Password=RecipeAI2024!Strong;"
  },
  "OpenAI": {
    "ApiKey": "sk-your-openai-api-key-here",
    "Model": "gpt-4o",
    "MaxTokens": 4000,
    "Temperature": 0.7
  }
}

# 3. Restore and run
cd src/RecipeAI.Web
dotnet restore
dotnet run
```

### With Visual Studio 2022

1. Open `RecipeAI.sln`
2. Set `RecipeAI.Web` as startup project
3. Update `appsettings.json` with your OpenAI API key
4. Run MySQL (Docker or local installation)
5. Press `F5` to run with debugging

## NuGet Packages

### RecipeAI.Infrastructure

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
<PackageReference Include="Microsoft.Agents.AI.OpenAI" Version="1.0.0-preview.260128.1" />
<PackageReference Include="OpenAI" Version="2.8.0" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="9.0.0" />
```

### RecipeAI.Application

```xml
<PackageReference Include="FluentValidation" Version="11.11.0" />
```

### RecipeAI.Web

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="9.0.0" />
```

**Note**: Microsoft.Agents.AI.OpenAI is currently in preview. All other packages are production-ready and compatible with .NET 9.

## Docker Commands

```bash
# Build images
docker compose build

# Start services (detached mode)
docker compose up -d

# View logs (follow mode)
docker compose logs -f web
docker compose logs -f db

# View logs (last 50 lines)
docker compose logs --tail=50 web

# Stop services
docker compose stop

# Stop and remove containers, volumes
docker compose down -v

# Rebuild from scratch (no cache)
docker compose build --no-cache

# Check running containers
docker compose ps

# Execute command in running container
docker compose exec web /bin/bash
```

## Development Commands

```bash
# Build entire solution
dotnet build

# Build specific project
dotnet build src/RecipeAI.Web/RecipeAI.Web.csproj

# Restore NuGet packages
dotnet restore

# Run web application
cd src/RecipeAI.Web
dotnet run

# Run with watch (hot reload)
cd src/RecipeAI.Web
dotnet watch run

# Clean build artifacts
dotnet clean

# Run tests (if implemented)
dotnet test

# Check for package updates
dotnet list package --outdated
```

## Recent Architecture Changes (Git History)

Recent refactoring improved Clean Architecture adherence:

```
3aff05b - Remove JsonResponseHelper from Infrastructure layer
8e96c6c - Remove AgentOrchestrator from Infrastructure layer  
9bcaff7 - Add project reference for Application layer
c0fa453 - Register AI agents by interface ⭐
7d7f30a - Implement MealPlanningDomainService ⭐
0c608ce - Add PlanningSessionRepository implementation
19ee8f9 - Move JsonResponseHelper to Application layer ⭐
3079cd9 - Add IAgentOrchestrator interface to Application layer ⭐
bc09ddc - Add IPlanningSessionRepository interface to Domain layer
3953080 - Move AgentOrchestrator to Application layer ⭐
```

### Key Improvements

1. **Agent Orchestration Moved to Application Layer**
   - `AgentOrchestrator` moved from Infrastructure to Application
   - Better separation: Application coordinates workflow, Infrastructure provides implementations

2. **Proper Dependency Injection**
   - Agents registered by interface (`IMealPlanningAgent`, etc.) instead of concrete types
   - Enables testing, mocking, and loose coupling

3. **Domain Service Bridge Pattern**
   - `MealPlanningDomainService` bridges Domain interface (`IMealPlanningService`) to Application orchestrator
   - Keeps Domain independent of Application logic

4. **JSON Helper Relocated**
   - `JsonResponseHelper` moved from Infrastructure to Application
   - Belongs with orchestration logic, not external services

5. **Interface Segregation**
   - Agent interfaces defined in Application layer
   - Implementations in Infrastructure layer
   - Clean contract-based design

## Technical Highlights

### Microsoft Agent Framework Integration

This project uses the **Microsoft.Agents.AI.OpenAI** preview package to implement AI agents:

```csharp
// Agent initialization (example from MealPlanningAgent)
var _agent = new OpenAIClient(apiKey)
    .GetChatClient(model)
    .AsAIAgent(
        name: "MealPlanner",
        instructions: SystemPrompts.MealPlannerAgent);

// Agent execution
var response = await _agent.RunAsync(userPrompt, cancellationToken);
```

**Benefits**:
- Structured agent pattern with system instructions
- Built-in conversation management
- Native async/await support
- Cancellation token support for long-running operations

### Clean Architecture Benefits

1. **Independent of Frameworks**: Domain layer has no external dependencies
2. **Testable**: Each layer can be tested independently
3. **Independent of UI**: Business logic doesn't depend on Blazor
4. **Independent of Database**: Can swap MySQL for SQL Server without changing Domain
5. **Independent of External Services**: AI agents can be replaced without changing Application logic

### Dependency Injection Setup

**Application Layer** (`AddApplication()`):
```csharp
services.AddScoped<IAgentOrchestrator, AgentOrchestrator>();
services.AddValidatorsFromAssemblyContaining<MealPlanRequestValidator>();
```

**Infrastructure Layer** (`AddInfrastructure()`):
```csharp
// Database
services.AddDbContext<RecipeAIDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Repositories
services.AddScoped<IMealPlanRepository, MealPlanRepository>();
services.AddScoped<IRecipeRepository, RecipeRepository>();
services.AddScoped<IPlanningSessionRepository, PlanningSessionRepository>();

// Domain Services
services.AddScoped<IMealPlanningService, MealPlanningDomainService>();

// AI Agents (registered by interface for DI)
services.AddScoped<IMealPlanningAgent, MealPlanningAgent>();
services.AddScoped<INutritionCriticAgent, NutritionCriticAgent>();
services.AddScoped<IBudgetOptimizerAgent, BudgetOptimizerAgent>();

// OpenAI Configuration
services.Configure<OpenAIOptions>(configuration.GetSection("OpenAI"));
```

### Database Schema

**Main Tables**:
- `MealPlans` - Generated meal plans
- `Recipes` - Individual recipes within plans
- `Ingredients` - Recipe ingredients with costs
- `PlanningSessions` - AI iteration tracking and audit logs

**Relationships**:
- One `MealPlan` has many `Recipes`
- One `Recipe` has many `Ingredients`
- One `PlanningSession` has one `MealPlan` (tracks generation process)

**Auto-initialization**: Schema created and seeded on first run (`DbInitializer.InitializeAsync()`)

## Troubleshooting

### Docker Build Issues

If you encounter DI errors like "Unable to resolve service for type 'IMealPlanningAgent'":
- Ensure agents are registered by interface in `DependencyInjection.cs`
- Rebuild Docker image: `docker compose build --no-cache`
- Check logs: `docker compose logs web --tail=100`

### OpenAI API Errors

If you get "Unauthorized" or "Invalid API key":
- Verify your API key in `appsettings.json` or `.env`
- Ensure key starts with `sk-`
- Check OpenAI account has credits

### JSON Parsing Errors

If you see "AI returned incomplete or invalid JSON":
- Increase `MaxTokens` in configuration (try 6000+)
- Reduce number of days in plan request
- Try `gpt-4o-mini` for faster responses with lower token usage

### Database Connection Issues

If app can't connect to MySQL:
- Verify connection string in `appsettings.json`
- For Docker: use `Server=db` (service name)
- For local: use `Server=localhost`
- Check MySQL container is running: `docker compose ps`

## What I Learned Building This

This project demonstrates several advanced .NET 9 and architecture concepts:

### Clean Architecture Implementation
- **Dependency Inversion**: Web → Infrastructure → Application → Domain (dependencies flow inward)
- **Interface Segregation**: Agent interfaces in Application, implementations in Infrastructure
- **Single Responsibility**: Each layer has clear, focused responsibilities
- **Domain Service Bridge Pattern**: `MealPlanningDomainService` bridges Domain interface to Application orchestrator

### Multi-Agent AI Pattern
- **Planner-Critic-Optimizer** workflow for quality assurance
- **Iterative Refinement** with feedback loops (up to 3 iterations)
- **Graceful Degradation** when AI responses fail or JSON is malformed
- **Structured Prompts** in Polish for better localized results
- **Audit Logging** with `PlanningSession` entity tracking all iterations

### Modern .NET 9 Features
- **Primary Constructors** for cleaner dependency injection (C# 13)
- **Blazor Server** with interactive rendering mode
- **EF Core 9** with MySQL provider (Pomelo 9.0.0)
- **Top-Level Statements** in Program.cs
- **File-Scoped Namespaces** throughout solution
- **Global Using Directives** for cleaner code

### Production-Ready Practices
- **Docker Compose** for one-command deployment with health checks
- **Configuration Management** with Options pattern (`IOptions<OpenAIOptions>`)
- **Structured Logging** for debugging and monitoring
- **FluentValidation** for request validation
- **Repository Pattern** for data access abstraction
- **Error Handling** with custom exceptions and graceful fallbacks

## Contributing

Contributions are welcome! Areas for improvement:

- [ ] Add unit tests for `AgentOrchestrator`
- [ ] Implement integration tests for AI agents
- [ ] Add Swagger/OpenAPI documentation
- [ ] Implement response caching for repeated requests
- [ ] Add user authentication and authorization
- [ ] Support more diet types (Paleo, Whole30, Gluten-Free)
- [ ] Multi-language support (currently Polish prompts)
- [ ] Export meal plans to PDF
- [ ] Shopping list generation grouped by store sections
- [ ] Meal prep scheduling and reminders
- [ ] Nutritional goal tracking over time
- [ ] Recipe rating and favorites system

## Author

**Michał Bąkiewicz**  
[GitHub](https://github.com/thekcr85) • [LinkedIn](https://linkedin.com/in/mbakiewicz)

**Project Repository**: [github.com/thekcr85/RecipeAI](https://github.com/thekcr85/RecipeAI)

### This Project Demonstrates

✅ **Clean Architecture** with proper layer separation and dependency inversion  
✅ **Multi-Agent AI** using Planner-Critic-Optimizer pattern  
✅ **Microsoft Agent Framework** (preview) for structured AI workflows  
✅ **Blazor Server** with interactive components (.NET 9)  
✅ **Entity Framework Core 9** with MySQL (Pomelo 9.0.0 provider)  
✅ **Docker Compose** for containerized deployment  
✅ **Domain-Driven Design** principles  
✅ **SOLID Principles** throughout the codebase  
✅ **Repository Pattern** for data access  
✅ **Options Pattern** for configuration management  

## License

MIT License - Open source demonstration project

---

**Ready to start?** → `docker compose up` 🚀

**Questions or issues?** → [Open an issue](https://github.com/thekcr85/RecipeAI/issues)

**Star this repo** if you found it helpful! ⭐
