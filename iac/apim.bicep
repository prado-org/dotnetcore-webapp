param apimName string
param location string

@allowed([
  'Consumption'
  'Developer'
  'Basic'
  'Basicv2'
  'Standard'
  'Standardv2'
  'Premium'
])
param skuName string = 'Developer'

@allowed([
  1
  2
  3
  4
  5
  6
  7
  8
  9
  10
])
param skuCount int = 1

param publisherEmail string
param publisherName string
param logAnalyticsWorkspaceId string

resource apim 'Microsoft.ApiManagement/service@2021-08-01' = {
  name: apimName
  location: location
  sku: {
    name: skuName
    capacity: skuName == 'Consumption' ? 0 : (skuName == 'Developer' ? 1 : skuCount) // Capacidade é 0 para 'Consumption', 1 para 'Developer' e skuCount para outras SKUs
  }
  properties: {
    publisherEmail: publisherEmail
    publisherName: publisherName
  }
}

resource apiManagementDiagnosticSettings 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  scope: apim
  name: 'apiManagementDiagnosticSettings'
  properties: {
    workspaceId: logAnalyticsWorkspaceId
    logs: [
      {
        category: 'GatewayLogs'
        enabled: true
      }
    ]
    metrics: [
      {
        category: 'AllMetrics'
        enabled: true
      }
    ]
  }
}
