# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY *.sln .
COPY AppointmentApp/*.csproj AppointmentApp/
RUN dotnet restore AppointmentApp/AppointmentApp.csproj

COPY . .
RUN dotnet build AppointmentApp/AppointmentApp.csproj -c Release -o /app/build
RUN dotnet publish AppointmentApp/AppointmentApp.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "AppointmentApp.dll"]
