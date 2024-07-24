param webAppName string
param location string
param environment string
param acrName string
param enableAks bool

targetScope = 'subscription'

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: 'rg-${webAppName}-${environment}'
  location: location
}

resource rgCommon 'Microsoft.Resources/resourceGroups@2021-04-01' existing = {
  name: 'rg-${acrName}'
}

module servicePlan './servicePlan.bicep' = {
  name: 'servicePlanModule'
  scope: rg
  params: {
    planName: 'plan-${webAppName}-${environment}'
    location: location
    sku: 'S3'
  }
}

module webApp './webApp.bicep' = {
  name: 'webApp'
  scope: rg
  params: {
    planId: servicePlan.outputs.servicePlanId
    webAppName: 'app-${webAppName}-${environment}'
    location: location
    linuxFxVersion: 'DOTNETCORE|6.0'
  }
}

module webApi './webApp.bicep' = {
  name: 'webApi'
  scope: rg
  params: {
    planId: servicePlan.outputs.servicePlanId
    webAppName: 'api-${webAppName}-${environment}'
    location: location
    linuxFxVersion: 'DOTNETCORE|6.0'
  }
}

module aks './kubernetes.bicep' = if(enableAks) {
  name: 'aks'
  scope: rg
  params: {
    clusterName: 'aks-${webAppName}-${environment}'
    location: location
    dnsPrefix: 'aks-${webAppName}-${environment}'
    agentCount: 1
    aksVersion: '1.28.9'
  }
}

module aksRoleAssigment './aksRoleAssignments.bicep' = if(enableAks) {
  name: 'aksRoleAssigment'
  scope: rgCommon
  params: {
    acrName: format('acr{0}', acrName)
    aksPrincipalId: aks.outputs.principalId
  }
}
