# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy csproj files
COPY Renteffy.Api/*.csproj Renteffy.Api/
COPY Renteffy.Application/*.csproj Renteffy.Application/
COPY Renteffy.Domain/*.csproj Renteffy.Domain/
COPY Renteffy.Infrastructure/*.csproj Renteffy.Infrastructure/
COPY Renteffy.Persistence/*.csproj Renteffy.Persistence/
COPY Renteffy.Shared/*.csproj Renteffy.Shared/

# Restore dependencies
RUN dotnet restore Renteffy.Api/Renteffy.Api.csproj

# Copy source code
COPY . .

WORKDIR /src/Renteffy.Api

RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000

ENTRYPOINT ["dotnet", "Renteffy.Api.dll"]