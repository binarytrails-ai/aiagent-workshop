namespace AIAgent.API.Agents
{
    public static class AgentFactory
    {
        public static IAgentConfig GetAgent(string agentName)
        {
            // For now, only TechSupportAgent is supported. Extend this as more agents are added.
            if (string.IsNullOrEmpty(agentName) || agentName == "TechSupportAgent")
            {
                return new TechSupportAgentConfig();
            }

            if (string.IsNullOrEmpty(agentName) || agentName == "ContosoBikeStoreAgent")
            {
                return new ContosoBikeStoreAgentConfig();
            }

            // Add more agent types here as needed.
            throw new ArgumentException($"Unknown agentId: {agentName}");
        }
    }
}
