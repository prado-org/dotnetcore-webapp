param webAppName string
param location string
param environment string
// teste
targetScope = 'subscription'

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: 'rg-${webAppName}-${environment}'
  location: location
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
