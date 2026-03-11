FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY src/MyApp/*.csproj ./src/MyApp/ 
RUN dotnet restore ./src/MyApp/MyApp.csproj

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

COPY --from=build /app/out .

EXPOSE 10000

ENTRYPOINT ["dotnet", "Renteffy.Api.dll"]
