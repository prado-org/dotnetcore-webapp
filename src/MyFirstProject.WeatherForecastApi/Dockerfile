# Use the official image as a parent image.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Set the working directory.
WORKDIR /app

# Copy csproj and restore dependencies.
COPY *.csproj ./

RUN dotnet restore MyFirstProject.WeatherForecastApi.csproj

# Copy the rest of the working directory contents.
COPY . ./

# Build the app
RUN dotnet build MyFirstProject.WeatherForecastApi.csproj --configuration Release --no-restore

# Publish the app
RUN dotnet publish MyFirstProject.WeatherForecastApi.csproj --no-build --configuration Release --output out

# Build runtime image.
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "MyFirstProject.WeatherForecastApi.dll"]