# Use .NET 9 SDK for building the project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY appointment_site_mongo.sln ./
COPY AppointmentApp/AppointmentApp.csproj AppointmentApp/

# Restore dependencies
RUN dotnet restore "AppointmentApp/AppointmentApp.csproj"

# Copy everything
COPY . .

# Build
RUN dotnet build "AppointmentApp/AppointmentApp.csproj" -c Release -o /app/build

# Publish
RUN dotnet publish "AppointmentApp/AppointmentApp.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AppointmentApp.dll"]
