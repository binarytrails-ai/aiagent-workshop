targetScope = 'subscription'

// Core parameters
@minLength(1)
@maxLength(64)
@description('Name of the environment that can be used as part of naming resource convention')
param environmentName string

@minLength(1)
@description('Primary location for all resources')
param location string

param resourcePrefix string = 'aiagentwks'
param deploymentTimestamp string = utcNow()

// Azure AI Service parameters
param modelName string = 'gpt-4o'
param modelFormat string = 'OpenAI'
param modelVersion string = '2024-11-20'
param modelSkuName string = 'GlobalStandard'
param modelCapacity int = 140
param modelLocation string = location

var abbr = json(loadTextContent('./abbreviations.json'))

var baseResourceName = '${resourcePrefix}-${environmentName}'
var uniqueSuffixValue = substring(uniqueString(baseResourceName, deploymentTimestamp), 0, 4)
var rgName = '${abbr.resourceGroups}${baseResourceName}'

var resourceNames = {
  aiServices: '${abbr.aiServicesAccounts}${uniqueSuffixValue}'
  storageAccount: toLower('${abbr.storageStorageAccounts}${replace(baseResourceName, '-', '')}${uniqueSuffixValue}')
  aiHub: '${abbr.aiFoundryHubs}${uniqueSuffixValue}'
  aiProject: '${abbr.aiFoundryAccounts}proj-${uniqueSuffixValue}'
}

var tags = {
  'azd-env-name': environmentName
}

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
    modelName: modelName
    modelFormat: modelFormat
    modelVersion: modelVersion
    modelSkuName: modelSkuName
    modelCapacity: modelCapacity
    modelLocation: modelLocation
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

output subscriptionId string = subscription().subscriptionId
output resourceGroupName string = rgName
output aiProjectName string = resourceNames.aiProject
