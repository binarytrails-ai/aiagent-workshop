@description('Azure region of the deployment')
param location string = resourceGroup().location

@description('Tags to add to the resources')
param tags object = {}

@description('AI services name')
param aiServicesName string

@description('Chat completion model name for deployment')
param modelName string

@description('Chat completion model format for deployment')
param modelFormat string

@description('Chat completion model version for deployment')
param modelVersion string

@description('Chat completion model deployment SKU name')
param modelSkuName string

@description('Chat completion model deployment capacity')
param modelCapacity int

@description('Model/AI Resource deployment location')
param modelLocation string

@description('Embedding model name for deployment')
param embeddingModelName string

@description('Embedding model format for deployment')
param embeddingModelFormat string

@description('Embedding model version for deployment')
param embeddingModelVersion string

@description('Embedding model deployment SKU name')
param embeddingModelSkuName string

@description('Embedding model deployment capacity')
param embeddingModelCapacity int

resource aiServices 'Microsoft.CognitiveServices/accounts@2024-10-01' = {
  name: aiServicesName
  location: modelLocation
  sku: {
    name: 'S0'
  }
  kind: 'AIServices'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    customSubDomainName: toLower(aiServicesName)
    publicNetworkAccess: 'Enabled'
  }
}



// resource chatCompletionModelDeployment 'Microsoft.CognitiveServices/accounts/deployments@2024-10-01' = {
//   parent: aiServices
//   name: modelName
//   sku: {
//     capacity: modelCapacity
//     name: modelSkuName
//   }
//   properties: {
//     model: {
//       name: modelName
//       format: modelFormat
//       version: modelVersion
//     }
//   }
// }

// resource embeddingModelDeployment 'Microsoft.CognitiveServices/accounts/deployments@2024-10-01' = {
//   parent: aiServices
//   name: embeddingModelName
//   sku: {
//     capacity: embeddingModelCapacity
//     name: embeddingModelSkuName
//   }
//   properties: {
//     model: {
//       name: embeddingModelName
//       format: embeddingModelFormat
//       version: embeddingModelVersion
//     }
//   }
// }

@description('Name of the storage account')
param storageName string

@allowed([
  'Standard_LRS'
  'Standard_ZRS'
  'Standard_GRS'
  'Standard_GZRS'
  'Standard_RAGRS'
  'Standard_RAGZRS'
  'Premium_LRS'
  'Premium_ZRS'
])
@description('Storage SKU')
param storageSkuName string = 'Standard_LRS'

resource storage 'Microsoft.Storage/storageAccounts@2023-01-01' = {
  name: storageName
  location: location
  tags: tags
  sku: {
    name: storageSkuName
  }
  kind: 'StorageV2'
  properties: {
    accessTier: 'Hot'
    allowBlobPublicAccess: false
    allowCrossTenantReplication: false
    allowSharedKeyAccess: true
    encryption: {
      keySource: 'Microsoft.Storage'
      requireInfrastructureEncryption: false
      services: {
        blob: {
          enabled: true
          keyType: 'Account'
        }
        file: {
          enabled: true
          keyType: 'Account'
        }
        queue: {
          enabled: true
          keyType: 'Service'
        }
        table: {
          enabled: true
          keyType: 'Service'
        }
      }
    }
    isHnsEnabled: false
    isNfsV3Enabled: false
    keyPolicy: {
      keyExpirationPeriodInDays: 7
    }
    largeFileSharesState: 'Disabled'
    minimumTlsVersion: 'TLS1_2'
    networkAcls: {
      bypass: 'AzureServices'
      defaultAction: 'Allow'
    }
    supportsHttpsTrafficOnly: true
  }
}

output aiServicesId string = aiServices.id
output aiServicesTarget string = aiServices.properties.endpoint
output aiServicesName string = aiServices.properties.customSubDomainName
output storageId string = storage.id
