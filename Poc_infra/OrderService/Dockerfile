# OrderService Dockerfile
# Multi-stage build for optimized production image

# Stage 1: Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies (separate layer for better caching)
COPY ["OrderService.csproj", "./"]
RUN dotnet restore "OrderService.csproj"

# Copy everything else and build
COPY . .
RUN dotnet build "OrderService.csproj" -c Release -o /app/build

# Stage 2: Publish stage
FROM build AS publish
RUN dotnet publish "OrderService.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Final runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install curl for health checks
USER root
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create non-root user for security
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copy published app from publish stage
COPY --from=publish /app/publish .

# Change ownership to non-root user
RUN chown -R appuser:appuser /app

# Switch to non-root user
USER appuser

# Expose port (will be mapped by docker-compose or runtime)
EXPOSE 8080
EXPOSE 8081

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

# Health check
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl --fail http://localhost:8080/health || exit 1

# Run the application
ENTRYPOINT ["dotnet", "OrderService.dll"]
