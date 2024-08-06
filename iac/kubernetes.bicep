// https://docs.microsoft.com/en-us/azure/aks/kubernetes-walkthrough-rm-template#create-an-ssh-key-pair
// https://github.com/Azure/azure-quickstart-templates/blob/master/quickstarts/microsoft.kubernetes/aks/main.bicep

@description('The name of the Managed Cluster resource.')
param clusterName string

@description('The location of the Managed Cluster resource.')
param location string

@description('Optional DNS prefix to use with hosted Kubernetes API server FQDN.')
param dnsPrefix string

@description('Disk size (in GB) to provision for each of the agent pool nodes. This value ranges from 0 to 1023. Specifying 0 will apply the default disk size for that agentVMSize.')
@minValue(0)
@maxValue(1023)
param osDiskSizeGB int = 0

@description('The number of nodes for the cluster.')
@minValue(1)
@maxValue(50)
param agentCount int = 2

@description('The size of the Virtual Machine.')
param agentVMSize string = 'Standard_D3_v2'

param aksVersion string = '1.25.6'

var logAnalyticsName = toLower('log-${clusterName}')

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2020-03-01-preview' = {
  name: logAnalyticsName
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 120
  }
}

resource aks 'Microsoft.ContainerService/managedClusters@2020-09-01' = {
  name: clusterName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    dnsPrefix: dnsPrefix
    nodeResourceGroup: 'rg-${clusterName}-infra'
    kubernetesVersion: aksVersion
    agentPoolProfiles: [
      {
        name: 'systemNodePool'
        osDiskSizeGB: osDiskSizeGB
        count: 1
        vmSize: agentVMSize
        osType: 'Linux'
        mode: 'System'
      }
      {
        name: 'userNodePool'
        osDiskSizeGB: osDiskSizeGB
        count: agentCount
        vmSize: agentVMSize
        osType: 'Linux'
        mode: 'User'
      }
    ]
    addonProfiles: {
      omsagent: {
        enabled: true
        config: {
          logAnalyticsWorkspaceResourceID: logAnalyticsWorkspace.id
        }
      }
    }
  }
}

output controlPlaneFQDN string = aks.properties.fqdn
output principalId string = aks.properties.identityProfile.kubeletidentity.objectId
