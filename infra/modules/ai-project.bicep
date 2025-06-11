@description('Azure region of the deployment')
param location string

@description('Tags to add to the resources')
param tags object

@description('AI Project name')
param aiProjectName string

@description('AI Project display name')
param aiProjectFriendlyName string = aiProjectName

@description('AI Project description')
param aiProjectDescription string

@description('Resource ID of the AI Hub resource')
param aiHubId string

@description('Name of the Azure OpenAI resource.')
param openAiServiceName string

@description('Name of the AI Search service.')
param aiSearchServiceName string

resource aiProject 'Microsoft.MachineLearningServices/workspaces@2024-10-01' = {
  name: aiProjectName
  location: location
  tags: tags
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    // organization
    friendlyName: aiProjectFriendlyName
    description: aiProjectDescription

    // dependent resources
    hubResourceId: aiHubId
  }
  kind: 'project'
}

output aiProjectId string = aiProject.id
output aiProjectName string = aiProject.name
output aiProjectPrincipalId string = aiProject.identity.principalId

// Move role assignments inside the project module
module aiServiceRoleAssignments 'ai-service-role-assignments.bicep' = {
  name: 'ai-service-roles'
  params: {
    aiServicesName: openAiServiceName
    aiProjectPrincipalId: aiProject.identity.principalId
    aiProjectId: aiProject.id
  }
}

module aiSearchRoleAssignments 'ai-search-role-assignments.bicep' = {
  name: 'ai-search-roles'
  params: {
    aiSearchName: aiSearchServiceName
    aiProjectPrincipalId: aiProject.identity.principalId
    aiProjectId: aiProject.id
  }
}
