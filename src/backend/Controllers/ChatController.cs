using AIAgent.API.Agents;
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
        private TechSupportAgentConfig _techSupportAgentConfig = new TechSupportAgentConfig();

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
                Name = _techSupportAgentConfig.AgentName,
                DisplayName = _techSupportAgentConfig.GetAgentDisplayName(),
                Description = _techSupportAgentConfig.GetAgentDescription(),
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
            var agentInstructions = _techSupportAgentConfig.GetAgentSystemMessage();
            var agentName = _techSupportAgentConfig.AgentName;

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
                    if (agentDefn.Name == agentName)
                        agentDefinition = agentDefn;
                }
            }

            if (agentDefinition == null)
            {
                agentDefinition = await _projectClient.Administration.CreateAgentAsync(
                  modelId,
                  name: agentName,
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
