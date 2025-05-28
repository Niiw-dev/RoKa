# Imagen base del SDK para desarrollo
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS dev

# Establece el directorio de trabajo
WORKDIR /app

# Copia el archivo de proyecto (esto puede ayudar al restore en cache)
COPY RoKa.csproj ./

# Restaura paquetes NuGet
RUN dotnet restore

# Copia el resto del código
COPY . .

# Expone el puerto usado por tu aplicación
EXPOSE 8080

# Comando para iniciar dotnet watch en modo desarrollo
CMD ["dotnet", "run", "--urls=http://0.0.0.0:8080"]
