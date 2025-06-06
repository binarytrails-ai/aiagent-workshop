targetScope = 'subscription'

// Core parameters
@minLength(1)
@maxLength(64)
@description('Name of the environment that can be used as part of naming resource convention')
param environmentName string

@minLength(1)
@description('Primary location for all resources')
param location string = 'australiaeast'

param resourcePrefix string = 'aiagentwks'

// Azure AI Service parameters
param chatCompletionModel string = 'gpt-4o'
param chatCompletionModelFormat string = 'OpenAI'
param chatCompletionModelVersion string = '2024-11-20'
param chatCompletionModelSkuName string = 'GlobalStandard'
param chatCompletionModelCapacity int = 50
param modelLocation string = 'eastus'

// Embedding model parameters
param embeddingModelName string = 'text-embedding-3-small'
param embeddingModelFormat string = 'OpenAI'
param embeddingModelVersion string = '2024-11-20'
param embeddingModelSkuName string = 'GlobalStandard'
param embeddingModelCapacity int = 350

// Load standard Azure abbreviations
var abbr = json(loadTextContent('./abbreviations.json'))

// Resource naming convention
var rgName = '${abbr.resourceGroups}${resourcePrefix}-${environmentName}'
var uniqueSuffixValue = substring(uniqueString(subscription().subscriptionId, rgName), 0, 6)

// Resource names
var resourceNames = {
  aiServices: '${abbr.aiServicesAccounts}${uniqueSuffixValue}'
  storageAccount: toLower('${abbr.storageStorageAccounts}${replace(uniqueSuffixValue, '-', '')}')
  aiHub: '${abbr.aiFoundryHubs}${uniqueSuffixValue}'
  aiProject: '${abbr.aiFoundryAccounts}proj-${uniqueSuffixValue}'
}

// Tags
var tags = {
  'azd-env-name': environmentName
  'azd-service-name': 'aiagent'
}

// Resource group
resource rg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: rgName
  location: location
  tags: tags
}

// Deploy dependent resources for the AI Hub
module aiDependencies 'modules/ai-services.bicep' = {
  scope: rg
  name: 'dep-${uniqueSuffixValue}'
  params: {
    aiServicesName: resourceNames.aiServices
    storageName: resourceNames.storageAccount
    location: location
    tags: tags

    // Model deployment parameters
    modelName: chatCompletionModel
    modelFormat: chatCompletionModelFormat
    modelVersion: chatCompletionModelVersion
    modelSkuName: chatCompletionModelSkuName
    modelCapacity: chatCompletionModelCapacity
    modelLocation: modelLocation

    // Embedding model parameters
    embeddingModelName: embeddingModelName
    embeddingModelFormat: embeddingModelFormat
    embeddingModelVersion: embeddingModelVersion
    embeddingModelSkuName: embeddingModelSkuName
    embeddingModelCapacity: embeddingModelCapacity
  }
}

// Deploy the AI Hub
module aiHub 'modules/ai-hub.bicep' = {
  scope: rg
  name: 'hub-${uniqueSuffixValue}'
  params: {
    aiHubName: resourceNames.aiHub
    aiHubFriendlyName: 'AI Agent Workshop Hub'
    aiHubDescription: 'AI Hub for the Azure AI Agent Workshop'
    location: location
    tags: tags

    // dependent resources
    modelLocation: modelLocation
    storageAccountId: aiDependencies.outputs.storageId
    aiServicesId: aiDependencies.outputs.aiServicesId
    aiServicesTarget: aiDependencies.outputs.aiServicesTarget
  }
}

// Deploy the AI Project
module aiProject 'modules/ai-project.bicep' = {
  scope: rg
  name: 'proj-${uniqueSuffixValue}'
  params: {
    aiProjectName: resourceNames.aiProject
    aiProjectFriendlyName: 'AI Agent Workshop Project'
    aiProjectDescription: 'AI Project for the Azure AI Agent Workshop'
    location: location
    tags: tags

    // dependent resources
    aiHubId: aiHub.outputs.aiHubId
  }
}

output AZURE_LOCATION string = location
output AZURE_TENANT_ID string = tenant().tenantId
output AZURE_SUBSCRIPTION_ID string = subscription().subscriptionId
output AZURE_RESOURCE_GROUP string = rg.name

output AZURE_AI_HUB_NAME string = aiHub.outputs.aiHubName
output AZURE_AI_HUB_ID string = aiHub.outputs.aiHubId
output AZURE_AI_PROJECT_NAME string = aiProject.outputs.aiProjectName
output AZURE_AI_PROJECT_ID string = aiProject.outputs.aiProjectId

// // AI Services outputs
output AZURE_AI_SERVICE_ENDPOINT string = aiDependencies.outputs.aiServicesTarget
output AZURE_AI_SERVICE_NAME string = aiDependencies.outputs.aiServicesName
output AZURE_STORAGE_ACCOUNT string = resourceNames.storageAccount
output AZURE_OPENAI_SERVICE_ENDPOINT string = 'https://${aiDependencies.outputs.aiServicesName}.openai.azure.com/'
// output AZURE_STORAGE_ID string = aiDependencies.outputs.storageId
