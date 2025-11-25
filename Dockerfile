# ASP.NET 8 runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# COPY csproj from AppointmentApp folder
COPY AppointmentApp/AppointmentApp.csproj AppointmentApp/

RUN dotnet restore AppointmentApp/AppointmentApp.csproj

# Copy all source code
COPY . .

WORKDIR "/src/AppointmentApp"
RUN dotnet build "AppointmentApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AppointmentApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AppointmentApp.dll"]
