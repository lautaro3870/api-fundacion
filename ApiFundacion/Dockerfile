# Etapa base: runtime
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

ARG CONNECTION_STRING
ENV CONNECTION_STRING=${CONNECTION_STRING}

EXPOSE 80
# Etapa build: SDK para compilar la aplicaci�n
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copiar el archivo .csproj y restaurar dependencias
COPY ["ApiFundacion/ApiFundacion.csproj", "ApiFundacion/"]
RUN dotnet restore "ApiFundacion/ApiFundacion.csproj"

# Copiar el resto del c�digo fuente y compilar
COPY . .
WORKDIR "/src/ApiFundacion"
RUN dotnet build "ApiFundacion.csproj" -c Release -o /app/build

# Etapa publish: publicar la aplicaci�n
FROM build AS publish
RUN dotnet publish "ApiFundacion.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final: runtime para ejecutar la aplicaci�n
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiFundacion.dll"]
