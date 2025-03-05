param webAppName string
param location string
param environment string
param acrName string
param enableAks bool
param enableApim bool
param dotnetVersion string = 'DOTNETCORE|8.0'

resource rgCommon 'Microsoft.Resources/resourceGroups@2021-04-01' existing = {
  scope: subscription()
  name: 'rg-${acrName}'
}

module servicePlan './servicePlan.bicep' = {
  name: 'servicePlanModule'
  params: {
    planName: 'plan-${webAppName}-${environment}'
    location: location
    sku: 'P1v3' //2 cores 8GB ram 250GB disk
  }
}

module webApp './webApp.bicep' = {
  name: 'webAppModule'
  params: {
    planId: servicePlan.outputs.servicePlanId
    webAppName: 'app-${webAppName}-${environment}'
    location: location
    linuxFxVersion: dotnetVersion
  }
}

module todoApi './webApp.bicep' = {
  name: 'todoApiModule'
  params: {
    planId: servicePlan.outputs.servicePlanId
    webAppName: 'api-todo-${environment}'
    location: location
    linuxFxVersion: dotnetVersion
  }
}

module weatherForecastApi './webApp.bicep' = {
  name: 'weatherForecastApiModule'
  params: {
    planId: servicePlan.outputs.servicePlanId
    webAppName: 'api-weatherforecast-${environment}'
    location: location
    linuxFxVersion: dotnetVersion
  }
}

module logWorkspace './LogAnalyticsWorkspace.bicep' = {
  name: 'logWorkspaceModule'
  params: {
    logAnalyticsName: 'log-${webAppName}-${environment}'
    location: location
  }
}

module aks './kubernetes.bicep' = if (enableAks) {
  name: 'aksModule'
  params: {
    clusterName: 'aks-${webAppName}-${environment}'
    location: location
    dnsPrefix: 'aks-${webAppName}-${environment}'
    agentCount: 1
    aksVersion: '1.31.5'
    logAnalyticsWorkspaceId: logWorkspace.outputs.logAnalyticsWorkspaceId
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
    logAnalyticsWorkspaceId: logWorkspace.outputs.logAnalyticsWorkspaceId
  }
}
