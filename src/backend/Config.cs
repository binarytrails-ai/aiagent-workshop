using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.Cosmos;
using System;

namespace AIAgent.API
{
    /// <summary>
    /// Static wrapper for AppConfig, similar to config_kernel.py in Python.
    /// Provides static access to configuration and helper methods for backward compatibility.
    /// </summary>
    public static class Config
    {
        private static AppConfig _appConfig;
        private static IServiceProvider _serviceProvider;
        private static CosmosClient _cosmosClient;
        private static Database _cosmosDatabase;

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _appConfig = serviceProvider.GetRequiredService<AppConfig>();
        }

        // Expose AppConfig properties as static for compatibility
        public static string AZURE_TENANT_ID => _appConfig.AzureTenantId;
        public static string AZURE_CLIENT_ID => _appConfig.AzureClientId;
        public static string AZURE_CLIENT_SECRET => _appConfig.AzureClientSecret;
        public static string COSMOSDB_ENDPOINT => _appConfig.CosmosDbEndpoint;
        public static string COSMOSDB_DATABASE => _appConfig.CosmosDbDatabase;
        public static string COSMOSDB_CONTAINER => _appConfig.CosmosDbContainer;
        public static string AZURE_OPENAI_DEPLOYMENT_NAME => _appConfig.AzureOpenAIDeploymentName;
        public static string AZURE_OPENAI_API_VERSION => _appConfig.AzureOpenAIApiVersion;
        public static string AZURE_OPENAI_ENDPOINT => _appConfig.AzureOpenAIEndpoint;
        public static string AZURE_OPENAI_SCOPE => _appConfig.AzureOpenAIScope;
        public static string FRONTEND_SITE_NAME => _appConfig.FrontendSiteName;
        public static string AZURE_AI_SUBSCRIPTION_ID => _appConfig.AzureAISubscriptionId;
        public static string AZURE_AI_RESOURCE_GROUP => _appConfig.AzureAIResourceGroup;
        public static string AZURE_AI_PROJECT_NAME => _appConfig.AzureAIProjectName;
        public static string AZURE_AI_AGENT_PROJECT_CONNECTION_STRING => _appConfig.AzureAIAgentProjectConnectionString;

        // Example: Get Cosmos DB client (expand as needed)
        public static CosmosClient GetCosmosClient()
        {
            if (_cosmosClient == null)
            {
                _cosmosClient = new CosmosClient(COSMOSDB_ENDPOINT, AZURE_CLIENT_SECRET);
            }
            return _cosmosClient;
        }

        public static Database GetCosmosDatabase()
        {
            if (_cosmosDatabase == null)
            {
                _cosmosDatabase = GetCosmosClient().GetDatabase(COSMOSDB_DATABASE);
            }
            return _cosmosDatabase;
        }

        // Add additional static helpers as needed for Azure credentials, OpenAI, etc.
    }
}
