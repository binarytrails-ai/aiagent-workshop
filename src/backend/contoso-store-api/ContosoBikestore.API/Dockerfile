# Dockerfile for .NET API
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ContosoStore.API/ContosoStore.API.csproj", "ContosoStore.API/"]
RUN dotnet restore "ContosoStore.API/ContosoStore.API.csproj"
COPY . .
WORKDIR "/src/ContosoStore.API"
RUN dotnet build "ContosoStore.API.csproj" -c Release -o /app/build
RUN dotnet publish "ContosoStore.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ContosoStore.API.dll"]
