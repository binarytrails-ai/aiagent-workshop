namespace AIAgent.API.Agents;

public class ContosoBikeStoreAgentConfig : AgentConfigBase
{
    public ContosoBikeStoreAgentConfig()
       : base("ContosoBikeStoreAgent")
    {
    }

    public override string GetAgentDisplayName()
    {
        return "Contoso Bike Store Agent";
    }

    public override string GetDescription()
    {
        return "This agent provides customer support for Contoso Bike Store. " +
                "It assists users with product inquiries, order status, and store information. " +
                "The agent utilizes the `MCP` tools to effectively address and resolve customer questions.";
    }

    public override string GetSystemMessage()
    {
        return "You are a customer support agent for Contoso Bike Store. " +
                "Your role is to assist users with product information, order status, and store details. " +
                "Use the tools available to you to provide accurate and helpful responses.";
    }
}


