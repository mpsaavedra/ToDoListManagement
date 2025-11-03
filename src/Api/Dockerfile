# --- ETAPA 1: Build ---
# Usamos el SDK de .NET 8.0 (o 9.0 si lo prefieres) para construir el proyecto.
# El nombre 'build' nos permite referenciar esta etapa más adelante.
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiamos todos los archivos .csproj y restauramos las dependencias.
# Al copiar solo los .csproj primero, Docker puede usar la caché si no han cambiado,
# acelerando futuras compilaciones.
COPY ["src/Api/Bootler.Api.csproj", "src/Api/"]
COPY ["src/Infrastructure/Bootler.Infrastructure.csproj", "src/Infrastructure/"]
COPY ["src/Domain/Bootler.Domain.csproj", "src/Domain/"]
COPY ["src/BuildingBlocks/Bootler.csproj", "src/BuildingBlocks/"]
COPY ["src/Contracts/Bootler.Contracts.csproj", "src/Contracts/"]
RUN dotnet restore "src/Api/Bootler.Api.csproj"

# Copiamos el resto del código fuente de todos los proyectos.
COPY . .
WORKDIR "/src/Api"
RUN dotnet publish "Bootler.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# --- ETAPA 2: Production ---
# Usamos la imagen de runtime de ASP.NET, que es mucho más ligera que la del SDK.
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
# Copiamos solo los artefactos publicados de la etapa de 'build'.
COPY --from=build /app/publish .
# El punto de entrada para ejecutar la API.
ENTRYPOINT ["dotnet", "Bootler.Api.dll"]