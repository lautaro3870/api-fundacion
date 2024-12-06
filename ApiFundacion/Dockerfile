# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiFundacion.csproj", "./"]
RUN dotnet restore "./ApiFundacion.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ApiFundacion.csproj" -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish "ApiFundacion.csproj" -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiFundacion.dll"]