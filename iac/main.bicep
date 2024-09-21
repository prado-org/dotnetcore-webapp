param webAppName string
param location string
param environment string
param acrName string
param enableAks bool
param enableApim bool

resource rgCommon 'Microsoft.Resources/resourceGroups@2021-04-01' existing = {
  scope: subscription()
  name: 'rg-${acrName}'
}

module servicePlan './servicePlan.bicep' = {
  name: 'servicePlanModule'
  params: {
    planName: 'plan-${webAppName}-${environment}'
    location: location
    sku: 'S3'
  }
}

module webApp './webApp.bicep' = {
  name: 'webAppModule'
  params: {
    planId: servicePlan.outputs.servicePlanId
    webAppName: 'app-${webAppName}-${environment}'
    location: location
    linuxFxVersion: 'DOTNETCORE|8.0'
  }
}

module webApi './webApp.bicep' = {
  name: 'webApiModule'
  params: {
    planId: servicePlan.outputs.servicePlanId
    webAppName: 'api-${webAppName}-${environment}'
    location: location
    linuxFxVersion: 'DOTNETCORE|8.0'
  }
}

module aks './kubernetes.bicep' = if (enableAks) {
  name: 'aksModule'
  params: {
    clusterName: 'aks-${webAppName}-${environment}'
    location: location
    dnsPrefix: 'aks-${webAppName}-${environment}'
    agentCount: 1
    aksVersion: '1.28.9'
  }
}

module aksRoleAssigment './aksRoleAssignments.bicep' = if (enableAks) {
  name: 'aksRoleAssigmentModule'
  scope: rgCommon
  params: {
    acrName: format('acr{0}', acrName)
    aksPrincipalId: enableAks ? aks.outputs.principalId : ''
  }
}

module apim './apim.bicep' = if (enableApim) {
  name: 'apimModule'
  params: {
    apimName: 'apim-${webAppName}-${environment}'
    location: location
    skuName: 'Developer'
    skuCount: 1
    publisherEmail: 'leadro@microsoft.com'
    publisherName: 'Leandro Prado'
  }
}
