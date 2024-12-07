# Etapa base
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Etapa de construcción
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiFundacion/ApiFundacion.csproj", "./"]
RUN dotnet restore "./ApiFundacion.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ApiFundacion/ApiFundacion.csproj" -c Release -o /app/build

# Etapa de publicación
FROM build AS publish
RUN dotnet publish "ApiFundacion/ApiFundacion.csproj" -c Release -o /app/publish

# Etapa final
FROM base AS final
WORKDIR /app

# Pasar el argumento CONNECTION_STRING al contenedor como variable de entorno
ARG CONNECTION_STRING
ENV CONNECTION_STRING=${CONNECTION_STRING}

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiFundacion.dll"]