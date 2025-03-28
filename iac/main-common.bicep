param acrName string
param webAppName string
param location string

module acr './containerRegistry.bicep' = {
  name: 'acrModule'
  params: {
    acrName: format('acr{0}', acrName)
    location: location
  }
}

module sqlServer './sqlServer.bicep' = {
  name: 'sqlServerModule'
  params: {
    sqlServerName: 'sql-${webAppName}'
    sqlAdministratorLogin: 'sqladmin'
    sqlAdministratorLoginPassword: '#P@ssw0rd123456#'
    sqlDatabaseName: 'db-${webAppName}'
    location: location
  }
}
