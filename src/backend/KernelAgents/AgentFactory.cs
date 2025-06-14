using AIAgent.API.Models;

namespace AIAgent.API.KernelAgents
{
    /// <summary>
    /// Factory for creating agents in the Multi-Agent Custom Automation Engine.
    /// </summary>
    public class AgentFactory
    {
        private readonly Dictionary<AgentType, Func<IAgent>> _agentRegistry = new();

        /// <summary>
        /// Register an agent type with the factory.
        /// </summary>
        public void RegisterAgent(AgentType type, Func<IAgent> factory)
        {
            _agentRegistry[type] = factory;
        }

        /// <summary>
        /// Create an agent of the specified type.
        /// </summary>
        public IAgent CreateAgent(AgentType type)
        {
            if (_agentRegistry.TryGetValue(type, out var factory))
                return factory();
            throw new ArgumentException($"Unknown agent type: {type}");
        }
    }
}
