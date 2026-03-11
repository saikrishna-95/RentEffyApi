# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and restore
COPY *.sln .
COPY Rentefy.Api/*.csproj Rentefy.Api/
COPY Rentefy.Application/*.csproj Rentefy.Application/
COPY Rentefy.Domain/*.csproj Rentefy.Domain/
COPY Rentefy.Infrastructure/*.csproj Rentefy.Infrastructure/
COPY Rentefy.Persistence/*.csproj Rentefy.Persistence/
COPY Rentefy.Shared/*.csproj Rentefy.Shared/

RUN dotnet restore

COPY . .

WORKDIR /src/Rentefy.Api
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000

ENTRYPOINT ["dotnet", "Rentefy.Api.dll"]