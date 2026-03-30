# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY src/NarutoGame.Core/NarutoGame.Core.csproj src/NarutoGame.Core/
COPY src/NarutoGame.Infrastructure/NarutoGame.Infrastructure.csproj src/NarutoGame.Infrastructure/
COPY src/NarutoGame.Application/NarutoGame.Application.csproj src/NarutoGame.Application/
COPY src/NarutoGame.API/NarutoGame.API.csproj src/NarutoGame.API/

# Restore dependencies
RUN dotnet restore src/NarutoGame.API/NarutoGame.API.csproj

# Copy all source code
COPY src/ src/

# Build the application
RUN dotnet build src/NarutoGame.API/NarutoGame.API.csproj -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish src/NarutoGame.API/NarutoGame.API.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install curl for healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create non-root user for security
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app

# Copy published files
COPY --from=publish --chown=appuser:appuser /app/publish .

# Switch to non-root user
USER appuser

# Expose port
EXPOSE 80

# Set environment variables from build args
ARG ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_ENVIRONMENT=${ASPNETCORE_ENVIRONMENT}
ENV ASPNETCORE_URLS=http://+:80

ENTRYPOINT ["dotnet", "NarutoGame.API.dll"]
