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
param embeddingModelCapacity int = 120

// Load standard Azure abbreviations
var abbr = json(loadTextContent('./abbreviations.json'))

// Resource naming convention
var rgName = '${abbr.resourceGroups}${resourcePrefix}-${environmentName}'
var uniqueSuffixValue = substring(uniqueString(subscription().subscriptionId, rgName), 0, 6)

// Resource names
var resourceNames = {
  aiService: toLower('${abbr.aiServicesAccounts}${uniqueSuffixValue}')
  keyVault: toLower('${abbr.keyVault}${uniqueSuffixValue}')
  storageAccount: toLower('${abbr.storageStorageAccounts}${replace(uniqueSuffixValue, '-', '')}')
  aiFoundryAccount: toLower('${abbr.aiFoundryAccounts}${uniqueSuffixValue}')
  aiFoundryProject: toLower('${abbr.aiFoundryAccounts}proj-${uniqueSuffixValue}')
  aiSearch: toLower('${abbr.aiSearchSearchServices}${replace(uniqueSuffixValue, '-', '')}')
  logAnalytics: toLower('log-${uniqueSuffixValue}')
  appInsights: toLower('appi-${uniqueSuffixValue}')
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

// Deploy Azure AI Search service as a module
module shared 'modules/shared.bicep' = {
  scope: rg
  name: 'search-${uniqueSuffixValue}'
  params: {
    aiSearchName: resourceNames.aiSearch
    storageAccountName: resourceNames.storageAccount
    keyVaultName: resourceNames.keyVault
    location: location
    tags: tags
    logAnalyticsWorkspaceName: resourceNames.logAnalytics
    appInsightsName: resourceNames.appInsights
  }
}

//Create AI Foundry Account
module aiFoundryAccount 'modules/ai-foundry-account.bicep' = {
  scope: rg
  name: 'foundry-${uniqueSuffixValue}'
  params: {
    name: resourceNames.aiFoundryAccount
    location: location
    tags: tags
  }
}

// Create AI Foundry Project
module aiProject 'modules/ai-project.bicep' = {
  scope: rg
  name: 'proj-${uniqueSuffixValue}'
  params: {
    name: resourceNames.aiFoundryProject
    location: location
    tags: tags
    aiFoundryName: aiFoundryAccount.outputs.name
  }
}

// Create the OpenAI Service
module aiDependencies 'modules/ai-services.bicep' = {
  scope: rg
  name: 'dep-${uniqueSuffixValue}'
  params: {
    aiServicesName: resourceNames.aiService
    location: location
    tags: tags

    aiFoundryAccountName: aiFoundryAccount.outputs.name
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

// Add App Service Plan
module app 'modules/app.bicep' = {
  scope: rg
  name: 'app-${uniqueSuffixValue}'
  params: {
    resourcePrefix: resourcePrefix
    uniqueSuffixValue: uniqueSuffixValue
    location: location
    tags: tags
    foundryProjectEndpoint: aiProject.outputs.endpoint
    foundryProjectName: aiProject.outputs.name
    openAIDeploymentName: chatCompletionModel
  }
}

output AZURE_LOCATION string = location
output AZURE_TENANT_ID string = tenant().tenantId
output AZURE_SUBSCRIPTION_ID string = subscription().subscriptionId
output AZURE_RESOURCE_GROUP string = rg.name
output AZURE_STORAGE_ACCOUNT string = resourceNames.storageAccount

output AZURE_AI_PROJECT_NAME string = aiProject.outputs.name
output AZURE_AI_PROJECT_ENDPOINT string = aiProject.outputs.endpoint

output BACKEND_APP_URL string = app.outputs.BACKEND_APP_URL
output FRONTEND_APP_URL string = app.outputs.FRONTEND_APP_URL
output CONTOSO_STORE_APP_URL string = app.outputs.CONTOSO_STORE_APP_URL
output CONTOSO_STORE_MCP_URL string = app.outputs.CONTOSO_STORE_MCP_URL

output AZURE_OPENAI_DEPLOYMENT_NAME string = chatCompletionModel
output TEXT_MODEL_NAME string = chatCompletionModel //TODO: to be removed when the notebook is updated
