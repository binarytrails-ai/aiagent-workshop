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

        public static void Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _appConfig = serviceProvider.GetRequiredService<AppConfig>();
        }

        // Expose AppConfig properties as static for compatibility
        public static string AZURE_TENANT_ID => _appConfig.AzureTenantId;
        public static string AZURE_OPENAI_DEPLOYMENT_NAME => _appConfig.AzureOpenAIDeploymentName;
        public static string AZURE_AI_SUBSCRIPTION_ID => _appConfig.AzureAISubscriptionId;
        public static string AZURE_AI_PROJECT_NAME => _appConfig.AzureAIProjectName;
        public static string AZURE_AI_PROJECT_ENDPOINT => _appConfig.AzureAIAgentProjectEndpoint;
        public static string FRONTEND_APP_URL => _appConfig.FrontendAppUrl;
        public static string CONTOSO_STORE_MCP_URL => _appConfig.ContosoStoreMcpUrl;
        public static string CONTOSO_STORE_MCP_SERVER_LABEL => !string.IsNullOrEmpty(_appConfig.ContosoStoreMcpServerLabel) ?
            _appConfig.ContosoStoreMcpServerLabel : "contosoBikeStore";
        public static string CONTOSO_STORE_MCP_SERVER_API_KEY => !string.IsNullOrEmpty(_appConfig.ContosoStoreMcpServerApiKey) ?
          _appConfig.ContosoStoreMcpServerApiKey : "contosoBikeStore";
    }
}
