# Dockerfile for .NET API
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["AIAgent.API/AIAgent.API.csproj", "AIAgent.API/"]
RUN dotnet restore "AIAgent.API/AIAgent.API.csproj"
COPY . .
WORKDIR "/src/AIAgent.API"
RUN dotnet build "AIAgent.API.csproj" -c Release -o /app/build
RUN dotnet publish "AIAgent.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AIAgent.API.dll"]
