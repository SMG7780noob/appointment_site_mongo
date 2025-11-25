FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src

# Copy csproj from correct folder
COPY AppointmentApp/AppointmentApp.csproj ./ 

# Restore
RUN dotnet restore

# Copy rest of the project
COPY . .

# Build
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AppointmentApp.dll"]
