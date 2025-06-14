// Module to deploy App Service Plan, Backend App Service, and Frontend App Service
param resourcePrefix string
param uniqueSuffixValue string
param location string
param tags object

resource appServicePlan 'Microsoft.Web/serverfarms@2022-03-01' = {
  name: '${resourcePrefix}-plan-${uniqueSuffixValue}'
  location: location
  sku: {
    name: 'B1'
    tier: 'Basic'
  }
  tags: tags
}

// Backend App Service
resource backendApp 'Microsoft.Web/sites@2022-03-01' = {
  name: '${resourcePrefix}-api-${uniqueSuffixValue}'
  location: location
  tags: union(tags, {
    'azd-service-name': 'api'
  })
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
  }
}

// Frontend App Service
resource frontendApp 'Microsoft.Web/sites@2022-03-01' = {
  name: '${resourcePrefix}-web-${uniqueSuffixValue}'
  location: location
  tags: union(tags, {
    'azd-service-name': 'web'
  })
  properties: {
    serverFarmId: appServicePlan.id
    httpsOnly: true
  }
}

output BACKEND_APP_URL string = 'https://${backendApp.name}.azurewebsites.net'
output FRONTEND_APP_URL string = 'https://${frontendApp.name}.azurewebsites.net'
