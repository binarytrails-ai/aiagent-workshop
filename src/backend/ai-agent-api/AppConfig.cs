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
        public string AzureOpenAIDeploymentName => GetRequiredEnvOrConfig("AZURE_OPENAI_DEPLOYMENT_NAME", "AzureOpenAI:DeploymentName", "gpt-4o");
        public string FrontendAppUrl => GetOptionalEnvOrConfig("FRONTEND_APP_URL", "FrontendAppUrl", "http://localhost:3000");
        public string AzureAISubscriptionId => GetRequiredEnvOrConfig("AZURE_AI_SUBSCRIPTION_ID", "AzureAI:SubscriptionId");
        public string AzureAIProjectName => GetRequiredEnvOrConfig("AZURE_AI_PROJECT_NAME", "AzureAI:ProjectName");
        public string AzureAIAgentProjectEndpoint => GetRequiredEnvOrConfig("AZURE_AI_PROJECT_ENDPOINT", "AzureAI:AgentProjectEndpoint");
        public string ContosoStoreMcpUrl => GetRequiredEnvOrConfig("CONTOSO_STORE_MCP_URL", "McpConfig:ContosoBikeStoreMcpUrl");
        public string ContosoStoreMcpServerLabel => GetOptionalEnvOrConfig("CONTOSO_STORE_MCP_SERVER_LABEL", "McpConfig:ContosoBikeStoreMcpServerLabel");

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
