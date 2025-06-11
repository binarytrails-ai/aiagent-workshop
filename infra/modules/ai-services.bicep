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

resource openAiService 'Microsoft.CognitiveServices/accounts@2024-10-01' = {
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

resource chatCompletionModelDeployment 'Microsoft.CognitiveServices/accounts/deployments@2024-10-01' = {
  parent: openAiService
  name: modelName
  sku: {
    capacity: modelCapacity
    name: modelSkuName
  }
  properties: {
    model: {
      name: modelName
      format: modelFormat
      version: modelVersion
    }
  }
}

resource embeddingModelDeployment 'Microsoft.CognitiveServices/accounts/deployments@2024-10-01' = {
  parent: openAiService
  dependsOn: [
    chatCompletionModelDeployment
  ]
  name: 'text-embedding-ada-002'
  sku: {
    capacity: embeddingModelCapacity
    name: embeddingModelSkuName
  }
  properties: {
    model: {
      name: 'text-embedding-ada-002'
      format: 'OpenAI'
      version: '2'
    }
  }
}

output openAiServiceResourceId string = openAiService.id
output openAiServiceEndpoint string = openAiService.properties.endpoint
output openAiServiceName string = openAiService.name
output openAiServiceDomain string = openAiService.properties.customSubDomainName
