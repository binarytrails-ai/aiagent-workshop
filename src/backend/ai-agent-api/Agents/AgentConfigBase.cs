namespace AIAgent.API.Agents
{
    public interface IAgentConfig
    {
        string AgentName { get; }
        string GetAgentDisplayName();
        string GetSystemMessage();
        string GetDescription();
    }

    public abstract class AgentConfigBase : IAgentConfig
    {
        private string _agentName;
        private string _systemMessage;

        protected AgentConfigBase(
            string agentName)
        {
            _agentName = agentName;
        }

        public string AgentName
        {
            get => _agentName;
        }

        public virtual string GetAgentDisplayName()
        {
            return _agentName;
        }

        public static string DefaultSystemMessage(string agentName = null)
        {
            return $"You are an AI assistant named {agentName}. Help the user by providing accurate and helpful information.";
        }

        public virtual string GetSystemMessage()
        {
            if (string.IsNullOrEmpty(_systemMessage))
            {
                _systemMessage = DefaultSystemMessage(_agentName);
            }
            return _systemMessage;
        }

        public virtual string GetDescription()
        {
            return $"This is a generic AI agent named {_agentName}. " +
                $"It is designed to assist users with various tasks and provide information based on the context of the conversation.";
        }
    }
}


