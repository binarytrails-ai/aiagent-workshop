using AIAgent.API.KernelTools;
using AIAgent.API.Models;
using Azure.AI.Agents.Persistent;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.AzureAI;

namespace AIAgent.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class ChatController : ControllerBase
    {
        private readonly PersistentAgentsClient _projectClient;
        private const string TechSupportAgentName = "TechSupportAgent";

        public ChatController()
        {
            var projectEndpoint = Config.AZURE_AI_PROJECT_ENDPOINT;
            _projectClient = AzureAIAgent.CreateAgentsClient(projectEndpoint, new DefaultAzureCredential());
        }

        // GET /agent
        [HttpGet("agent")]
        public IActionResult GetChatAgent()
        {
            var agentInfo = new
            {
                Name = TechSupportAgentName,
                Description = """
                This agent provides IT and technical support for the company. 
                It assists users with troubleshooting, technical queries, and problem resolution. 
                The agent utilizes the `TechSupportTools` to effectively address and resolve technical issues.
                """
            };
            return Ok(agentInfo);
        }

        // GET /chat/history
        [HttpGet("chat/history")]
        public async Task<IActionResult> History([FromQuery] string? agentId, [FromQuery] string? threadId)
        {
            if (string.IsNullOrEmpty(threadId))
                return Ok(new List<ChatMessageHistory>()); // No history for this thread

            var messages = await ChatUtils.GetChatMessageHistoryAsync(threadId, _projectClient);
            return Ok(messages);
        }

        [HttpPost("chat/send")]
        public async Task<IActionResult> ChatSend([FromBody] ChatRequest request)
        {
            var agent = await GetOrCreateAgentAsync(request.AgentId);
            var agentThread = await ChatUtils.GetOrCreateAgentThreadAsync(request.ThreadId, _projectClient);
            await ChatUtils.InvokeAgent(request.Message, agent, agentThread);
            var response = new ChatCompletionResponse
            {
                AgentId = agent.Id,
                ThreadId = agentThread.Id ?? string.Empty
            };

            return Ok(response);
        }


        private async Task<AzureAIAgent> GetOrCreateAgentAsync(string? agentId)
        {
            PersistentAgent agentDefinition = null;
            var modelId = Config.AZURE_OPENAI_DEPLOYMENT_NAME;
            var agentInstructions = """
            You are a helpful technical support assistant for the company. Your primary responsibility is to assist users with IT support and technical queries.
            Always use the `TechSupportTools` tool to help resolve technical issues when appropriate.
            If a user asks for information or assistance outside of technical support, politely decline and encourage them to ask about IT support or technical matters.
            If you are unable to assist with a request, respond courteously and let the user know you can only help with technical support issues.
            Be clear, concise, and professional in all your responses.
            """;

            if (!string.IsNullOrEmpty(agentId))
            {
                try
                {
                    agentDefinition = await _projectClient.Administration.GetAgentAsync(agentId);
                }
                catch (Azure.RequestFailedException)
                {
                    // Agent not found, will create a new one
                }
            }

            if (agentDefinition == null)
            {
                await foreach (var agentDefn in _projectClient.Administration.GetAgentsAsync())
                {
                    if (agentDefn.Name == TechSupportAgentName)
                        agentDefinition = agentDefn;
                }
            }

            if (agentDefinition == null)
            {
                agentDefinition = await _projectClient.Administration.CreateAgentAsync(
                  modelId,
                  name: TechSupportAgentName,
                  instructions: agentInstructions,
                  tools: [],
                  toolResources: null);
            }

            var agent = new AzureAIAgent(agentDefinition, _projectClient);
            agent.Kernel.ImportPluginFromObject(new TechSupportTools(), "TechSupportTools");
            return agent;
        }
    }
}
