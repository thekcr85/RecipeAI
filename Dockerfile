# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["src/RecipeAI.Web/RecipeAI.Web.csproj", "RecipeAI.Web/"]
COPY ["src/RecipeAI.Application/RecipeAI.Application.csproj", "RecipeAI.Application/"]
COPY ["src/RecipeAI.Infrastructure/RecipeAI.Infrastructure.csproj", "RecipeAI.Infrastructure/"]
COPY ["src/RecipeAI.Domain/RecipeAI.Domain.csproj", "RecipeAI.Domain/"]

# Restore dependencies
RUN dotnet restore "RecipeAI.Web/RecipeAI.Web.csproj"

# Copy source code
COPY src/ .

# Build and publish
WORKDIR "/src/RecipeAI.Web"
RUN dotnet publish "RecipeAI.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Copy published app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "RecipeAI.Web.dll"]
