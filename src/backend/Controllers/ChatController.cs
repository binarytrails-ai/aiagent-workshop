using AIAgent.API.Models;
using Azure.AI.Agents.Persistent;
using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.AzureAI;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AIAgent.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class ChatController : ControllerBase
    {
        private readonly PersistentAgentsClient _projectClient;
        private const string ContosoInventoryAgentName = "BikeStoreInventoryAgent";

        public ChatController()
        {
            var projectEndpoint = "https://aif-mbo43n.services.ai.azure.com/api/projects/aif-proj-mbo43n";
            _projectClient = AzureAIAgent.CreateAgentsClient(projectEndpoint, new DefaultAzureCredential());
        }

        // GET /agent
        [HttpGet("agent")]
        public IActionResult GetChatAgent()
        {
            var agentInfo = new
            {
                Name = ContosoInventoryAgentName,
                Description = "This agent assists with bike inventory inquiries at Contoso Bike Store."
            };
            return Ok(agentInfo);
        }

        // GET /chat/history
        [HttpGet("chat/history")]
        public async Task<IActionResult> History()
        {
            var threadId = Request.Cookies["thread_id"];
            var agentId = Request.Cookies["agent_id"];

            if (string.IsNullOrEmpty(threadId))
                return Ok(new List<object>()); // No history for this thread

            var agent = await GetOrCreateAgentAsync(agentId);

            var persistentThread = await _projectClient.Threads.GetThreadAsync(threadId);
            if (persistentThread == null)
                return Ok(new List<object>()); // No history for this thread

            var agentThread = new AzureAIAgentThread(_projectClient, persistentThread.Value.Id);
            var messages = new List<ChatMessageHistory>();
            await foreach (var msg in _projectClient.Messages.GetMessagesAsync(agentThread.Id))
            {
                foreach (var content in msg.ContentItems)
                {
                    if (content is MessageTextContent)
                    {

                        messages.Add(new ChatMessageHistory
                        {
                            Role = msg.Role == Azure.AI.Agents.Persistent.MessageRole.User ? "user" : "assistant",
                            Content = (content as MessageTextContent).Text,
                            CreatedAt = msg.CreatedAt.ToString("o") // ISO 8601 format
                        });
                    }
                }
            }

            return Ok(messages);
        }

        // POST /chat/send
        [HttpPost("chat/send")]
        public async Task<IActionResult> ChatSend([FromBody] ChatRequest request)
        {
            var threadId = Request.Cookies["thread_id"];
            var agentId = Request.Cookies["agent_id"];

            var agent = await GetOrCreateAgentAsync(agentId);

            AzureAIAgentThread agentThread = null;
            if (!string.IsNullOrEmpty(threadId))
            {
                var persistentThread = await _projectClient.Threads.GetThreadAsync(threadId);
                if (persistentThread != null)
                    agentThread = new AzureAIAgentThread(_projectClient, persistentThread.Value.Id);
            }

            agentThread ??= new AzureAIAgentThread(_projectClient);
            var userMessage = new ChatMessageContent(AuthorRole.User, request.Message);
            await foreach (var response in agent.InvokeAsync(userMessage, agentThread))
            {
            }

            // Set cookies for thread and agent IDs
            Response.Cookies.Append("thread_id", agentThread.Id);
            Response.Cookies.Append("agent_id", agent.Id);

            return Ok();
        }

        private async Task<AzureAIAgent> GetOrCreateAgentAsync(string agentId)
        {
            PersistentAgent agentDefinition = null;
            var modelId = "gpt-4o";
            var agentInstructions = """
            You are a helpful assistant for Contoso Bike Store. Use the BikeInventoryPlugin to answer questions about bike availability and features.
            If the user asks for a bike that is not in stock, inform them politely.
            If the user asks for a list of available bikes, provide the list.
            If the user asks for other information, politely decline and suggest they ask about bikes in the store.
            If the user asks for bike features, provide a short description if available.
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
                    if (agentDefn.Name == ContosoInventoryAgentName)
                        agentDefinition = agentDefn;
                }
            }

            if (agentDefinition == null)
            {
                agentDefinition = await _projectClient.Administration.CreateAgentAsync(
                  modelId,
                  name: ContosoInventoryAgentName,
                  instructions: agentInstructions,
                  tools: [],
                  toolResources: null);
            }

            return new AzureAIAgent(agentDefinition, _projectClient);
        }
    }
}
