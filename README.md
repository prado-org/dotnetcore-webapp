# My First Project

> [!WARNING]
> This code is used for example purposes only.

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
docker-compose --file 'DockerCompose.yml' up --build --detach
docker-compose --file 'DockerCompose.yml' ps
docker-compose --file 'DockerCompose.yml' down

# List containers running
docker ps -a
```

## ARGO CD

https://argo-cd.readthedocs.io/en/stable/getting_started/
https://techcommunity.microsoft.com/t5/apps-on-azure-blog/getting-started-with-gitops-argo-and-azure-kubernetes-service/ba-p/3288595
https://igboie.medium.com/kubernetes-ci-cd-with-github-github-actions-and-argo-cd-36b88b6bda64
https://medium.com/@tanmaybhandge/ci-cd-with-github-github-actions-argo-cd-and-kubernetes-cluster-192b019129f6

```powershell
$rg_name="rg-argocd"
$location="brazilsouth"
$aks_name="aks-argocd"
$aks_namespace="argocd"

# Create Resource Group
az group create --name $rg_name --location $location

# Create AKS cluster
az aks create --resource-group $rg_name --name $aks_name --node-count 1 --generate-ssh-keys

# Connect to AKS cluster
az aks get-credentials --resource-group $rg_name --name $aks_name

# Create AKS namespace
kubectl create namespace $aks_namespace

# Install ARGOCD
kubectl apply -n $aks_namespace -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml

# Get ARGOCD admin password
kubectl get secret argocd-initial-admin-secret -n $aks_namespace -o json | ConvertFrom-Json | select -ExpandProperty data | % { $_.PSObject.Properties | % { $_.Name + [System.Environment]::NewLine + [System.Text.Encoding]::UTF8.GetString([System.Convert]::FromBase64String($_.Value)) + [System.Environment]::NewLine + [System.Environment]::NewLine } }

# Access with localhost
kubectl port-forward svc/argocd-server -n $aks_namespace 8080:443
https://localhost:8080/
user:admin

#Access with Load Balancer - demonstration purposes only
kubectl patch service argocd-server -n $aks_namespace -p '{\"spec\": {\"type\": \"LoadBalancer\"}}'
kubectl get services --namespace $aks_namespace argocd-server --output jsonpath='{.status.loadBalancer.ingress[0].ip}'

https://172.214.7.210/
user:admin

# Install ARGOCD CLI
$version = (Invoke-RestMethod https://api.github.com/repos/argoproj/argo-cd/releases/latest).tag_name

$url = "https://github.com/argoproj/argo-cd/releases/download/" + $version + "/argocd-windows-amd64.exe"
$output = "C:\argocd\argocd.exe"

Invoke-WebRequest -Uri $url -OutFile $output

[Environment]::SetEnvironmentVariable("Path", "$env:Path;C:\argocd", "User")

# Login Argocd Server
argocd login 172.214.7.210 --username admin

argocd account list

argocd cluster list

# You can only register a new K8s cluster from the Argo CD CLI
# Login AKS Cluster
az aks get-credentials --resource-group rg-dotnetproject-dev --name aks-dotnetproject-dev

# List AKS contexts
kubectl config get-contexts -o name

# Add AKS cluster to ArgoCD
argocd cluster add aks-dotnetproject-dev

# List clusters
argocd cluster list

# Add private GitRepo to Argocd
argocd repo add https://github.com/prado-org/dotnetcore-webapp-config.git --name dotnetcore-webapp --username git --password <GIT_HUB_PAT>

# List repos
argocd repo list

# Create argocd app
argocd app create dotnetcore-webapp-dev --repo https://github.com/prado-org/dotnetcore-webapp-config.git --path ./ --dest-namespace default --dest-server <AKS SERVER URL> --sync-policy automated --directory-recurse

# List argocd app
argocd app list

# Sync an app
argocd app sync dotnetcore-webapp

# Status an app
argocd app get dotnetcore-webapp

# Test the Sync - Change the image label directly Kubernetes server
kubectl config current-context
kubectl config set-context aks-dotnetproject-dev
kubectl edit deployment -n default myfirstproject-api-deploy
-he
```

## Authentication using Service Principal

```json
{
  "appId": "47e5a3d7-e460-47cf-a9c9-d359beba4f8d",
  "displayName": "myServicePrincipalTest",
  "password": "rJe8Q~Hdb3nYkT6HmNoxJuBjwnJwfuivsVzlsdyM",
  "tenant": "16b3c013-d300-468d-ac64-7eda0820b6d3"
}
```

## APIM
```powershell
$headers = @{
  "Ocp-Apim-Subscription-Key" = "7b000d597ffd47048bbf55cc8c67aaef"
}

for ($i = 1; $i -le 10; $i++) 
{
  Write-Output "Request $i"
  Invoke-RestMethod -Uri "https://apim-dotnetproject-dev.azure-api.net/todoapi/api/Categories" -Headers $headers
}

for ($i = 1; $i -le 10; $i++) 
{
  Write-Output "Request $i"
  Invoke-RestMethod -Uri "https://apim-dotnetproject-dev.azure-api.net/todoapi/api/TodoItem" -Headers $headers
}

```

## Contribute

Contributions are always welcome!

Please read the [contribution guidelines](CONTRIBUTING.md) first.

