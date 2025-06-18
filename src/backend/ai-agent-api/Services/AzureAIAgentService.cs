using Azure.Identity;
using Azure.AI.Agents.Persistent;
using Microsoft.SemanticKernel.Agents.AzureAI;
using AIAgent.API.Models;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel;
using AIAgent.API.KernelTools;
using AIAgent.API.Agents;

namespace AIAgent.API.Services
{
    /// <summary>
    /// Service implementation for Azure AI Agent operations.
    /// </summary>
    public class AzureAIAgentService : IAzureAIAgentService
    {
        private readonly PersistentAgentsClient _projectClient;
        private readonly ILogger<AzureAIAgentService> _logger;

        public AzureAIAgentService(ILogger<AzureAIAgentService> logger)
        {
            var projectEndpoint = Config.AZURE_AI_PROJECT_ENDPOINT;
            _projectClient = AzureAIAgent.CreateAgentsClient(projectEndpoint, new DefaultAzureCredential());
            _logger = logger;
        }

        public async Task<List<ChatMessageHistory>> GetChatMessageHistoryAsync(string threadId)
        {
            _logger.LogInformation("Fetching chat message history for threadId: {ThreadId}", threadId);
            if (string.IsNullOrEmpty(threadId))
            {
                _logger.LogWarning("No threadId provided for chat history.");
                return new();
            }

            var persistentThread = await _projectClient.Threads.GetThreadAsync(threadId);
            if (persistentThread == null)
            {
                _logger.LogWarning("No persistent thread found for threadId: {ThreadId}", threadId);
                return new();
            }

            var agentThread = new AzureAIAgentThread(_projectClient, persistentThread.Value.Id);
            var messages = new List<ChatMessageHistory>();
            var messagesAsync = _projectClient.Messages.GetMessagesAsync(agentThread.Id);
            var enumerator = messagesAsync.GetAsyncEnumerator();
            try
            {
                while (await enumerator.MoveNextAsync())
                {
                    var msg = enumerator.Current;
                    foreach (var content in msg.ContentItems)
                    {
                        if (content is MessageTextContent textContent)
                        {
                            messages.Add(new ChatMessageHistory
                            {
                                Role = msg.Role == Azure.AI.Agents.Persistent.MessageRole.User ? "user" : "assistant",
                                Content = textContent.Text,
                                CreatedAt = msg.CreatedAt.ToString("o")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching messages for threadId: {ThreadId}", threadId);
                throw;
            }
            finally
            {
                await enumerator.DisposeAsync();
            }
            _logger.LogInformation("Fetched {Count} messages for threadId: {ThreadId}", messages.Count, threadId);
            return messages;
        }

        public async Task<SendMessageResult> SendMessage(string userPrompt, string agentName, string agentId, string threadId)
        {
            _logger.LogInformation("Sending message to agent. agentId: {AgentId}, threadId: {ThreadId}", agentId, threadId);

            var agent = await GetOrCreateAgentAsync(agentName, agentId);
            var agentThread = await GetOrCreateAgentThreadAsync(threadId);

            await InvokeAgent(userPrompt, agent, agentThread, _logger);

            _logger.LogInformation("Message sent successfully. agentId: {AgentId}, threadId: {ThreadId}", agentId, agentThread.Id);
            return new SendMessageResult
            {
                ThreadId = agentThread.Id,
                AgentId = agent.Id
            };
        }

        public async Task<AzureAIAgentThread> GetOrCreateAgentThreadAsync(string? threadId)
        {
            _logger.LogInformation("Getting or creating agent thread for threadId: {ThreadId}", threadId);
            if (!string.IsNullOrEmpty(threadId))
            {
                var persistentThread = await _projectClient.Threads.GetThreadAsync(threadId);
                if (persistentThread != null)
                {
                    _logger.LogInformation("Found existing thread for threadId: {ThreadId}", threadId);
                    return new AzureAIAgentThread(_projectClient, persistentThread.Value.Id);
                }
            }
            _logger.LogInformation("Creating new agent thread.");
            return new AzureAIAgentThread(_projectClient);
        }

        private async Task<AzureAIAgent> GetOrCreateAgentAsync(string agentName, string? agentId)
        {
            _logger.LogInformation("Getting or creating agent for agentId: {AgentId}", agentId);
            PersistentAgent agentDefinition = null;
            var modelId = Config.AZURE_OPENAI_DEPLOYMENT_NAME;
            var agentConfig = AgentFactory.GetAgent(agentName);
            var agentInstructions = agentConfig.GetSystemMessage();

            if (!string.IsNullOrEmpty(agentId))
            {
                try
                {
                    agentDefinition = await _projectClient.Administration.GetAgentAsync(agentId);
                    _logger.LogInformation("Found existing agent for agentId: {AgentId}", agentId);
                }
                catch (Azure.RequestFailedException)
                {
                    _logger.LogWarning("Agent not found for agentId: {AgentId}, will create a new one.", agentId);
                }
            }

            if (agentDefinition == null)
            {
                var agentsAsync = _projectClient.Administration.GetAgentsAsync();
                var enumerator = agentsAsync.GetAsyncEnumerator();
                try
                {
                    while (await enumerator.MoveNextAsync())
                    {
                        var agentDefn = enumerator.Current;
                        if (agentDefn.Name == agentName)
                        {
                            agentDefinition = agentDefn;
                            _logger.LogInformation("Found agent by name: {AgentName}", agentName);
                            break;
                        }
                    }
                }
                finally
                {
                    await enumerator.DisposeAsync();
                }
            }

            if (agentDefinition == null)
            {
                _logger.LogInformation("Creating new agent with name: {AgentName}", agentName);
                agentDefinition = await _projectClient.Administration.CreateAgentAsync(
                    modelId,
                    name: agentName,
                    instructions: agentInstructions,
                    tools: [],
                    toolResources: null);
            }

            var agent = new AzureAIAgent(agentDefinition, _projectClient);
            agent.Kernel.ImportPluginFromObject(new TechSupportTools(), "TechSupportTools");
            _logger.LogInformation("Agent ready for use. agentId: {AgentId}", agentDefinition.Id);
            return agent;
        }

        private async Task InvokeAgent(string userPrompt, AzureAIAgent agent, AzureAIAgentThread agentThread, ILogger logger)
        {
            logger.LogInformation("Invoking agent for threadId: {ThreadId}", agentThread.Id);
            var userMessage = new ChatMessageContent(AuthorRole.User, userPrompt);
            var responsesAsync = agent.InvokeAsync(userMessage, agentThread);
            var enumerator = responsesAsync.GetAsyncEnumerator();
            try
            {
                while (await enumerator.MoveNextAsync())
                {
                    // No-op, just consume the responses
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while invoking agent for threadId: {ThreadId}", agentThread.Id);
                throw;
            }
            finally
            {
                await enumerator.DisposeAsync();
            }
            logger.LogInformation("Agent invocation completed for threadId: {ThreadId}", agentThread.Id);
        }

    }
}
