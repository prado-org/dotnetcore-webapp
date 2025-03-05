# Use the official image as a parent image.
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

# Set the working directory.
WORKDIR /app

# Copy csproj and restore dependencies.
COPY *.csproj ./

# Copy nuget.config from the parent directory
COPY nuget.config ./

RUN dotnet restore MyFirstProject.TodoApi.csproj

# Copy the rest of the working directory contents.
COPY . ./

# Build the app
RUN dotnet build MyFirstProject.TodoApi.csproj --configuration Release --no-restore

# Publish the app
RUN dotnet publish MyFirstProject.TodoApi.csproj --no-build --configuration Release --output out

# Build runtime image.
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "MyFirstProject.TodoApi.dll"]