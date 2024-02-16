# My First Project

In this tutorial for C# development with ASP.NET Core, you create a C# ASP.NET Core web app in Visual Studio.

This tutorial shows you how to:

- :eight_spoked_asterisk: Create a Visual Studio project
- :eight_spoked_asterisk: Create a C# ASP.NET Core web app
- :eight_spoked_asterisk: Explore IDE features
- :eight_spoked_asterisk: Run the web app

## Status
[![dotnetcore-webapp-ci-cd](https://github.com/prado-org/dotnetcore-webapp/actions/workflows/ci-cd.yml/badge.svg)](https://github.com/prado-org/dotnetcore-webapp/actions/workflows/ci-cd.yml)

[![dotnetcore-webapp-iac](https://github.com/prado-org/dotnetcore-webapp/actions/workflows/iac.yml/badge.svg)](https://github.com/prado-org/dotnetcore-webapp/actions/workflows/iac.yml)

[![dotnetcore-webapp-ci-cd-k8s](https://github.com/prado-org/dotnetcore-webapp/actions/workflows/ci-cd-k8s.yml/badge.svg)](https://github.com/prado-org/dotnetcore-webapp/actions/workflows/ci-cd-k8s.yml)

## Projects
- :arrow_right: src/MyFirstProject.WebApp
- :arrow_right: src/MyFirstProject.WebApi
- :arrow_right: src/MyFirstProject.Tests

## Build Process

```
# Restore Packages
dotnet restore src/MyFirstProject.sln

# Build Solution
dotnet build src/MyFirstProject.sln --configuration Debug --no-restore

# Running Unit Tests
dotnet test src/MyFirstProject.Tests/MyFirstProject.Tests.csproj --no-build --configuration Debug --verbosity normal --logger "trx;LogFileName=test-results.trx"

# Publish WebApp
dotnet publish src/MyFirstProject.WebApp/MyFirstProject.WebApp.csproj --no-build --configuration Debug --output PublishApp

# Publish WebApi
dotnet publish src/MyFirstProject.WebApi/MyFirstProject.WebApi.csproj --no-build --configuration Debug --output PublishApi

```

## Docker image
    
To build and run your Docker image, you can use the following commands:

```
# Build Docker images
cd .\src\MyFirstProject.WebApp\
docker build -t myfirstproject.webapp .

cd .\src\MyFirstProject.WebApi\
docker build -t myfirstproject.webapi .

# List images
docker images

# Run docker compose
cd .\src
docker-compose -f 'DockerCompose.yml' up --build -d

# List containers running
docker ps -a
```


## Contribute

Contributions are always welcome!

Please read the [contribution guidelines](CONTRIBUTING.md) first.
