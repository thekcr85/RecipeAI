# RecipeAI 🍽️

> AI-powered meal planning with multi-agent refinement using OpenAI GPT-4o

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4)](https://blazor.net/)
[![OpenAI](https://img.shields.io/badge/OpenAI-GPT--4o-412991)](https://openai.com/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1)](https://www.mysql.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED)](https://www.docker.com/)

## What This Does

A Blazor Server app that generates personalized meal plans with AI multi-agent collaboration:

- 🤖 **Planner Agent** generates meal plans strictly following diet type (Vegan, Keto, Mediterranean, etc.)
- 🔍 **Nutrition Critic** validates macros, overall balance, AND diet compliance (catches violations like dairy in vegan diets)
- 💰 **Budget Optimizer** keeps plans within budget and suggests diet-compliant alternatives
- 🔁 Iterative refinement (up to 3 iterations) with graceful degradation
- 📊 Complete nutrition summaries with macro breakdown
- 💾 Plans saved in MySQL with full iteration logs
- 🛡️ Automatic JSON validation and repair for incomplete AI responses

### Key Features

- **Strict Diet Compliance**: Automatically rejects plans with diet violations (e.g., eggs in vegan diet)
- **Multi-Agent Quality Assurance**: Three specialized agents collaborate to ensure perfect plans
- **Graceful Error Handling**: If refinement fails, system uses previous valid version
- **Production-Ready**: Built with Microsoft Agent Framework (preview) and .NET 9

## Tech Stack

```
.NET 9 + C# 13
Blazor Server (Interactive)
OpenAI GPT-4o (Microsoft Agent Framework)
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
# 1. Clone
git clone https://github.com/thekcr85/RecipeAI.git
cd RecipeAI

# 2. Create .env file with your API key
cp .env.example .env
# Edit .env and add your OpenAI API key

# 3. Start
docker compose up

# 4. Open browser
# → http://localhost:5000
```

That's it! The app will:
- Start MySQL database
- Create schema automatically (EnsureCreated)
- Seed 10 sample recipes
- Launch Blazor app on port 5000

## Screenshots

### Home Page
*Add screenshot here*

### Create Plan Form
*Add screenshot here*

### Meal Plan Details
*Add screenshot here*

### Plan List
*Add screenshot here*

## Local Development

Without Docker:

```bash
# 1. Start MySQL
docker run -d -p 3306:3306 \
  -e MYSQL_ROOT_PASSWORD=RecipeAI2024!Strong \
  -e MYSQL_DATABASE=RecipeAI \
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
    "Temperature": 0.7,
    "MaxIterations": 3
  }
}

# 3. Run
cd src/RecipeAI.Web
dotnet run
```

## Project Structure

```
src/
├── RecipeAI.Domain/              # 🎯 Core (Entities, Enums, Interfaces)
├── RecipeAI.Application/         # 💼 Business Logic
├── RecipeAI.Infrastructure/      # 🔧 Data + AI Services
└── RecipeAI.Web/                 # 🎨 Blazor UI
    └── Components/Pages/
        ├── Home.razor           # Landing page
        ├── CreatePlan.razor     # Meal plan generator
        ├── MealPlans.razor      # Plan list
        └── MealPlanDetails.razor # Plan details
```

**Clean Architecture** - dependencies flow inward (Web → Infra → App → Domain)

## How Multi-Agent Refinement Works

```
User Request: Diet=Vegan, Days=7, Calories=2000, Budget=500PLN

┌─────────────────────────────────────────────────────────────┐
│ Iteration 1: Initial Generation                            │
├─────────────────────────────────────────────────────────────┤
│ 🤖 Planner:   Generates 21 recipes (7 days × 3 meals)     │
│ 🔍 Critic:    ❌ "Found cow's milk in recipe 3 (vegan)"    │
│               ❌ "Found eggs in recipe 7 (vegan)"          │
│ 💰 Optimizer: ❌ "Total cost 580 PLN exceeds budget"       │
└─────────────────────────────────────────────────────────────┘
         ↓ Feedback: Remove dairy/eggs, reduce costs
┌─────────────────────────────────────────────────────────────┐
│ Iteration 2: Refinement                                    │
├─────────────────────────────────────────────────────────────┤
│ 🤖 Planner:   Replaces milk→soy milk, eggs→tofu           │
│ 🔍 Critic:    ✅ "All ingredients vegan-compliant"         │
│               ✅ "Macros within range (P:20% C:55% F:25%)" │
│ 💰 Optimizer: ❌ "Still 520 PLN, suggest cheaper options"  │
└─────────────────────────────────────────────────────────────┘
         ↓ Feedback: Optimize costs while keeping vegan
┌─────────────────────────────────────────────────────────────┐
│ Iteration 3: Final Polish                                  │
├─────────────────────────────────────────────────────────────┤
│ 🤖 Planner:   Applies budget optimizations                 │
│ 🔍 Critic:    ✅ "Approved - vegan + balanced"             │
│ 💰 Optimizer: ✅ "Within budget: 485 PLN"                  │
└─────────────────────────────────────────────────────────────┘
         ↓
    💾 Save to Database (3 iterations, all validation passed)
```

### Agent Responsibilities

**🤖 Meal Planning Agent**
- Generates initial meal plan with recipes and ingredients
- STRICTLY follows diet type restrictions (e.g., no dairy/eggs for vegan)
- Refines plans based on critic and optimizer feedback
- Uses 4000 tokens to generate complete JSON responses

**🔍 Nutrition Critic Agent**  
- **PRIMARY**: Validates diet compliance (blocks dairy in vegan, meat in vegetarian, etc.)
- Checks macro distribution (protein, carbs, fats)
- Ensures daily calorie targets are met (±10%)
- Verifies meal variety and balance
- Returns detailed feedback with `dietViolations` array

**💰 Budget Optimizer Agent**
- Checks total cost against budget limit
- Suggests cheaper alternatives (while respecting diet type!)
- Recommends seasonal and bulk-buy options
- Provides savings estimates

### Quality Safeguards

1. **Diet Violation Detection**: Critic agent automatically rejects plans with forbidden ingredients
2. **JSON Validation**: Automatic validation and repair of incomplete AI responses
3. **Graceful Degradation**: If refinement fails, uses previous valid version (no crashes)
4. **Iteration Logging**: Full audit trail of all agent decisions saved to database

## Features by Diet Type

### 🌱 Vegan Diet
- **Strictly plant-based**: No meat, fish, dairy, eggs, honey
- Uses: soy milk, oat milk, almond milk, tofu, tempeh, legumes
- Critic validates EVERY ingredient for animal products
- Example violations caught: cow's milk, butter, eggs, cheese, honey

### 🥗 Vegetarian Diet
- **No meat or fish**: Dairy and eggs allowed
- Uses: milk, cheese, yogurt, eggs, legumes, grains
- Critic blocks: meat, poultry, fish, seafood

### 🥑 Keto Diet
- **Very low carb** (<10% of calories), high fat (>65%)
- Focuses on: avocado, olive oil, nuts, seeds, meat, fish, low-carb vegetables
- Critic blocks: bread, rice, pasta, potatoes, most fruits

### 🌊 Mediterranean Diet
- **Heart-healthy**: Olive oil, fish, vegetables, fruits, nuts, whole grains
- Limited red meat
- Emphasis on healthy fats and omega-3

### 💪 High Protein Diet
- **Protein >30%** of daily calories
- Sources: lean meat, fish, eggs, legumes, dairy
- Ideal for muscle building and weight management

### 🍞 Low Carb Diet
- **Carbs <25%** of daily calories
- Reduced grains, sugars, starches
- Focus on proteins and healthy fats

## Pages

| Route | Description |
|-------|-------------|
| `/` | Landing page with app overview |
| `/create-plan` | AI meal plan generator form |
| `/meal-plans` | List all created plans |
| `/meal-plan/{id}` | Plan details with recipes and nutrition |

## Configuration

### Environment Variables (.env)

```bash
OPENAI_API_KEY=your-openai-api-key
OPENAI_MODEL=gpt-4o
OPENAI_MAX_TOKENS=4000
OPENAI_TEMPERATURE=0.7
OPENAI_MAX_ITERATIONS=3
MYSQL_PASSWORD=your-mysql-password
```

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=RecipeAI;User=root;Password=your_password;"
  },
  "OpenAI": {
    "ApiKey": "your-openai-api-key-here",
    "Model": "gpt-4o",
    "MaxTokens": 2000,
    "Temperature": 0.7,
    "MaxIterations": 3
  }
}
```

## NuGet Packages

### RecipeAI.Infrastructure

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="9.0.0" />
<PackageReference Include="Microsoft.Agents.AI.OpenAI" Version="1.0.0-preview.260128.1" />
<PackageReference Include="OpenAI" Version="2.8.0" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="9.0.0" />
```

**All packages are production-ready and compatible with .NET 9!**  
**Note:** Microsoft.Agents.AI.OpenAI is currently in preview.

## Docker Commands

```bash
# Start services
docker compose up

# Run in background
docker compose up -d

# View logs
docker compose logs -f web

# Stop & remove
docker compose down -v

# Rebuild
docker compose build --no-cache
```

## Development Commands

```bash
# Build solution
dotnet build

# Restore packages
dotnet restore

# Run locally
cd src/RecipeAI.Web
dotnet run

# Clean build artifacts
dotnet clean
```

## Author

**Michał Bąkiewicz** • [GitHub](https://github.com/thekcr85)

Project demonstrating:
- **Clean Architecture** with proper layer separation
- **Multi-Agent AI** using Planner-Critic-Optimizer pattern with Microsoft Agent Framework
- **Blazor Server** with interactive components (.NET 9)
- **Docker containerization** for one-command deployment
- **Entity Framework Core 9** with MySQL (Pomelo 9.0.0 provider)
- **Microsoft Agent Framework** (preview) for structured AI agent workflows

**Project Repository**: [github.com/thekcr85/RecipeAI](https://github.com/thekcr85/RecipeAI)

---

## License

MIT License - Open source demonstration project

---

**Get Started:** `docker compose up` 🚀
