# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and restore
COPY *.sln .
COPY Renteffy.Api/*.csproj Renteffy.Api/
COPY Renteffy.Application/*.csproj Renteffy.Application/
COPY Renteffy.Domain/*.csproj Renteffy.Domain/
COPY Renteffy.Infrastructure/*.csproj Renteffy.Infrastructure/
COPY Renteffy.Persistence/*.csproj Renteffy.Persistence/
COPY Renteffy.Shared/*.csproj Renteffy.Shared/

RUN dotnet restore

# Copy everything else
COPY . .

WORKDIR /src/Renteffy.Api
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000

ENTRYPOINT ["dotnet", "Renteffy.Api.dll"]