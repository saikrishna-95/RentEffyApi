# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and restore
COPY *.sln .
COPY Renteffy.Api/*.csproj Renteffy.Api/
RUN dotnet restore

# Copy everything else
COPY . .

WORKDIR /src/Renteffy.Api
RUN dotnet publish -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000

ENTRYPOINT ["dotnet", "Renteffy.Api.dll"]
