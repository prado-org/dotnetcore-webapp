# My First Project Test
Nova Feature
In this tutorial for C# development with ASP.NET Core, you create a C# ASP.NET Core web app in Visual Studio.

This tutorial shows you how to:

- :eight_spoked_asterisk: Create a Visual Studio project
- :eight_spoked_asterisk: Create a C# ASP.NET Core web app
- :eight_spoked_asterisk: Explore IDE features
- :eight_spoked_asterisk: Run the web app

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

## Contribute

Contributions are always welcome!

Please read the [contribution guidelines](contributing.md) first.
