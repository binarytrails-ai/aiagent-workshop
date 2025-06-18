namespace AIAgent.API.Agents
{
    public class TechSupportAgentConfig : AgentConfigBase
    {
        public TechSupportAgentConfig()
           : base("TechSupportAgent")
        {
        }

        public override string GetAgentDisplayName()
        {
            return "Technical Support Agent";
        }

        public override string GetDescription()
        {
            return "This agent provides IT and technical support for the company. " +
                   "It assists users with troubleshooting, technical queries, and problem resolution. " +
                   "The agent utilizes the `TechSupportTools` to effectively address and resolve technical issues.";
        }

        public override string GetSystemMessage()
        {
            return "You are a technical support agent. " +
                   "Your role is to assist users with IT and technical issues, providing solutions and troubleshooting steps. " +
                   "Use the tools available to you to resolve problems efficiently.";
        }
    }
}


