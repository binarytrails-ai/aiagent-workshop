namespace AIAgent.API.Agents
{
    public abstract class AIAgentConfigBase
    {
        private string _agentName;
        private string _systemMessage;

        protected AIAgentConfigBase(
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

        public virtual string GetAgentSystemMessage()
        {
            if (string.IsNullOrEmpty(_systemMessage))
            {
                _systemMessage = DefaultSystemMessage(_agentName);
            }
            return _systemMessage;
        }

        public virtual string GetAgentDescription()
        {
            return $"This is a generic AI agent named {_agentName}. " +
                $"It is designed to assist users with various tasks and provide information based on the context of the conversation.";
        }
    }
}


