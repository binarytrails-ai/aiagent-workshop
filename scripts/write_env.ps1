# Define the .env file path
$envFilePath = "src\.env"

# Clear the contents of the .env file
Set-Content -Path $envFilePath -Value ""

# Append new values to the .env file
$azureEnvName = azd env get-value AZURE_ENV_NAME
$azureAIProjectEndpoint = $env:AZURE_AI_PROJECT_ENDPOINT
$azureAIProjectName = $env:AZURE_AI_PROJECT_NAME
$azureAIServiceName = $env:AZURE_AI_SERVICE_NAME
$azureAppInsightsConnectionString = $env:AZURE_APP_INSIGHTS_CONNECTION_STRING
$azureAppInsightsInstrumentationKey = $env:AZURE_APP_INSIGHTS_INSTRUMENTATION_KEY
$azureEnvName = $env:AZURE_ENV_NAME
$azureLocation = $env:AZURE_LOCATION
$azureLogAnalyticsWorkspaceName = $env:AZURE_LOG_ANALYTICS_WORKSPACE_NAME
$azureResourceGroup = $env:AZURE_RESOURCE_GROUP
$azureSearchServiceEndpoint = $env:AZURE_SEARCH_SERVICE_ENDPOINT
$azureSearchServiceName = $env:AZURE_SEARCH_SERVICE_NAME
$azureStorageAccount = $env:AZURE_STORAGE_ACCOUNT
$azureSubscriptionId = $env:AZURE_SUBSCRIPTION_ID
$azureTenantId = $env:AZURE_TENANT_ID

Add-Content -Path $envFilePath -Value "AZURE_ENV_NAME=$azureEnvName"
Add-Content -Path $envFilePath -Value "AZURE_AI_PROJECT_ENDPOINT=$azureAIProjectEndpoint"
Add-Content -Path $envFilePath -Value "AZURE_AI_PROJECT_NAME=$azureAIProjectName"
Add-Content -Path $envFilePath -Value "AZURE_AI_SERVICE_NAME=$azureAIServiceName"
Add-Content -Path $envFilePath -Value "AZURE_APP_INSIGHTS_CONNECTION_STRING=$azureAppInsightsConnectionString"
Add-Content -Path $envFilePath -Value "AZURE_APP_INSIGHTS_INSTRUMENTATION_KEY=$azureAppInsightsInstrumentationKey"
Add-Content -Path $envFilePath -Value "AZURE_LOCATION=$azureLocation"
Add-Content -Path $envFilePath -Value "AZURE_LOG_ANALYTICS_WORKSPACE_NAME=$azureLogAnalyticsWorkspaceName"
Add-Content -Path $envFilePath -Value "AZURE_RESOURCE_GROUP=$azureResourceGroup"
Add-Content -Path $envFilePath -Value "AZURE_SEARCH_SERVICE_ENDPOINT=$azureSearchServiceEndpoint"
Add-Content -Path $envFilePath -Value "AZURE_SEARCH_SERVICE_NAME=$azureSearchServiceName"
Add-Content -Path $envFilePath -Value "AZURE_STORAGE_ACCOUNT=$azureStorageAccount"
Add-Content -Path $envFilePath -Value "AZURE_SUBSCRIPTION_ID=$azureSubscriptionId"
Add-Content -Path $envFilePath -Value "AZURE_TENANT_ID=$azureTenantId"

# Write-Host "🌐 Please visit web app URL:"
# Write-Host $serviceAPIUri -ForegroundColor Cyan