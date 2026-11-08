FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["RaceDay2.csproj", "./"]
RUN dotnet restore "RaceDay2.csproj"
COPY . .
RUN dotnet build "RaceDay2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RaceDay2.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RaceDay2.dll"]