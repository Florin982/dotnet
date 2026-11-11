# Fase 1: compilazione con l'SDK .NET 8
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY backend.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app --no-restore

# Fase 2: immagine leggera con il solo runtime ASP.NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENV PORT=10000
EXPOSE 10000
ENTRYPOINT ["dotnet", "backend.dll"]
