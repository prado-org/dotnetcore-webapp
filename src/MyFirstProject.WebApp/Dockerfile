# Use the official image as a parent image.
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

# Set the working directory.
WORKDIR /app

# Copy csproj and restore dependencies.
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the working directory contents.
COPY . ./

# Build the app.
RUN dotnet build

# Publish the app.
RUN dotnet publish --no-build --output out

# Build runtime image.
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "MyFirstProject.WebApp.dll"]