# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/Portfolio.Api/Portfolio.Api.csproj", "Portfolio.Api/"]
COPY ["src/Portfolio.Application/Portfolio.Application.csproj", "Portfolio.Application/"]
COPY ["src/Portfolio.Domain/Portfolio.Domain.csproj", "Portfolio.Domain/"]
COPY ["src/Portfolio.Infrastructure/Portfolio.Infrastructure.csproj", "Portfolio.Infrastructure/"]
COPY ["src/Portfolio.Persistence/Portfolio.Persistence.csproj", "Portfolio.Persistence/"]
COPY ["src/Portfolio.Shared/Portfolio.Shared.csproj", "Portfolio.Shared/"]

RUN dotnet restore "Portfolio.Api/Portfolio.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/Portfolio.Api"
RUN dotnet publish "Portfolio.Api.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Portfolio.Api.dll"]
