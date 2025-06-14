namespace AIAgent.API
{
    /// <summary>
    /// Application configuration loader for .NET, similar to Python's app_config.py.
    /// </summary>
    public class AppConfig
    {
        private readonly IConfiguration _config;

        public AppConfig(IConfiguration config)
        {
            _config = config;
        }

        // Azure authentication
        public string AzureTenantId => GetOptionalEnvOrConfig("AZURE_TENANT_ID", "Azure:TenantId");
        public string AzureClientId => GetOptionalEnvOrConfig("AZURE_CLIENT_ID", "Azure:ClientId");
        public string AzureClientSecret => GetOptionalEnvOrConfig("AZURE_CLIENT_SECRET", "Azure:ClientSecret");

        // CosmosDB
        public string CosmosDbEndpoint => GetOptionalEnvOrConfig("COSMOSDB_ENDPOINT", "CosmosDb:Endpoint");
        public string CosmosDbDatabase => GetOptionalEnvOrConfig("COSMOSDB_DATABASE", "CosmosDb:Database");
        public string CosmosDbContainer => GetOptionalEnvOrConfig("COSMOSDB_CONTAINER", "CosmosDb:Container");

        // Azure OpenAI
        public string AzureOpenAIDeploymentName => GetRequiredEnvOrConfig("AZURE_OPENAI_DEPLOYMENT_NAME", "AzureOpenAI:DeploymentName", "gpt-4o");
        public string AzureOpenAIApiVersion => GetRequiredEnvOrConfig("AZURE_OPENAI_API_VERSION", "AzureOpenAI:ApiVersion", "2024-11-20");
        public string AzureOpenAIEndpoint => GetRequiredEnvOrConfig("AZURE_OPENAI_ENDPOINT", "AzureOpenAI:Endpoint");
        public string AzureOpenAIScope => GetOptionalEnvOrConfig("AZURE_OPENAI_SCOPE", "AzureOpenAI:Scope", "https://cognitiveservices.azure.com/.default");

        // Frontend
        public string FrontendSiteName => GetOptionalEnvOrConfig("FRONTEND_SITE_NAME", "Frontend:SiteName", "http://127.0.0.1:3000");

        // Azure AI
        public string AzureAISubscriptionId => GetRequiredEnvOrConfig("AZURE_AI_SUBSCRIPTION_ID", "AzureAI:SubscriptionId");
        public string AzureAIResourceGroup => GetRequiredEnvOrConfig("AZURE_AI_RESOURCE_GROUP", "AzureAI:ResourceGroup");
        public string AzureAIProjectName => GetRequiredEnvOrConfig("AZURE_AI_PROJECT_NAME", "AzureAI:ProjectName");
        public string AzureAIAgentProjectConnectionString => GetRequiredEnvOrConfig("AZURE_AI_AGENT_PROJECT_CONNECTION_STRING", "AzureAI:AgentProjectConnectionString");

        private string GetRequiredEnvOrConfig(string envVar, string configKey, string defaultValue = null)
        {
            var value = Environment.GetEnvironmentVariable(envVar);
            if (!string.IsNullOrEmpty(value)) return value;
            value = _config[configKey];
            if (!string.IsNullOrEmpty(value)) return value;
            if (defaultValue != null) return defaultValue;
            throw new InvalidOperationException($"Missing required configuration: {envVar} or {configKey}");
        }

        private string GetOptionalEnvOrConfig(string envVar, string configKey, string defaultValue = "")
        {
            var value = Environment.GetEnvironmentVariable(envVar);
            if (!string.IsNullOrEmpty(value)) return value;
            value = _config[configKey];
            if (!string.IsNullOrEmpty(value)) return value;
            return defaultValue;
        }
    }
}
