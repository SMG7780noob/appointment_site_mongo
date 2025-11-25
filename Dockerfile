# Build Stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy csproj (root me hai)
COPY AppointmentApp.csproj ./

# Restore dependencies
RUN dotnet restore

# Copy everything
COPY . .

# Build and publish
RUN dotnet publish -c Release -o /app/publish


# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AppointmentApp.dll"]
