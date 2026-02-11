# RecipeAI ???

> AI-powered meal planning with multi-agent refinement using OpenAI GPT-4o

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4)](https://dotnet.microsoft.com/)
[![Blazor](https://img.shields.io/badge/Blazor-Server-512BD4)](https://blazor.net/)
[![OpenAI](https://img.shields.io/badge/OpenAI-GPT--4o-412991)](https://openai.com/)
[![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1)](https://www.mysql.com/)
[![Docker](https://img.shields.io/badge/Docker-Ready-2496ED)](https://www.docker.com/)

## What This Does

A Blazor Server app that generates personalized meal plans with AI multi-agent collaboration:

- ?? **Planner Agent** generates meal plans for a chosen diet and goals
- ?? **Nutrition Critic** validates macros and overall balance
- ?? **Budget Optimizer** keeps plans within budget and suggests alternatives
- ?? Iterative refinement (up to 3 iterations)
- ?? Nutrition summaries with macro breakdown
- ?? Plans saved in MySQL

## Tech Stack

```
.NET 9 + C# 13
Blazor Server (Interactive)
OpenAI GPT-4o (Microsoft.Extensions.AI)
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
# ? http://localhost:5000
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
    "MaxTokens": 2000,
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
??? RecipeAI.Domain/              # ?? Core (Entities, Enums, Interfaces)
??? RecipeAI.Application/         # ?? Business Logic
??? RecipeAI.Infrastructure/      # ?? Data + AI Services
??? RecipeAI.Web/                 # ?? Blazor UI
    ??? Components/Pages/
        ??? Home.razor           # Landing page
        ??? CreatePlan.razor     # Meal plan generator
        ??? MealPlans.razor      # Plan list
        ??? MealPlanDetails.razor # Plan details
```

**Clean Architecture** - dependencies flow inward (Web ? Infra ? App ? Domain)

## How Multi-Agent Refinement Works

```
User Request: Diet=Keto, Days=7, Calories=2000, Budget=500PLN

???????????????????????????????????????
? Iteration 1                         ?
???????????????????????????????????????
? Planner: initial plan               ?
? Critic: ? macros off                ?
? Optimizer: ? budget too high         ?
???????????????????????????????????????
         ?
???????????????????????????????????????
? Iteration 2                         ?
???????????????????????????????????????
? Planner: adjusted plan              ?
? Critic: ? approved                  ?
? Optimizer: ? within budget          ?
???????????????????????????????????????
         ?
    Final plan saved
```

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
OPENAI_MAX_TOKENS=2000
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
<PackageReference Include="Microsoft.Extensions.AI" Version="10.3.0" />
<PackageReference Include="Microsoft.Extensions.AI.OpenAI" Version="10.3.0" />
<PackageReference Include="OpenAI" Version="2.8.0" />
<PackageReference Include="Pomelo.EntityFrameworkCore.MySql" Version="9.0.0" />
```

**All packages are production-ready and compatible with .NET 9!**

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

**Micha³ B¹kiewicz** • [GitHub](https://github.com/thekcr85)

Project demonstrating:
- **Clean Architecture** with proper layer separation
- **Multi-Agent AI** using Planner-Critic-Optimizer pattern
- **Blazor Server** with interactive components (.NET 9)
- **Docker containerization** for one-command deployment
- **Entity Framework Core 9** with MySQL (Pomelo 9.0.0 provider)

**Project Repository**: [github.com/thekcr85/RecipeAI](https://github.com/thekcr85/RecipeAI)

---

## License

MIT License - Open source demonstration project

---

**Get Started:** `docker compose up` ??
