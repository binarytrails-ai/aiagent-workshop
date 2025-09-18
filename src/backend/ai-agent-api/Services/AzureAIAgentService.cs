using Azure.Identity;
using Azure.AI.Agents.Persistent;
using Microsoft.SemanticKernel.Agents.AzureAI;
using AIAgent.API.Models;
using AIAgent.API.Agents;
using Azure;

namespace AIAgent.API.Services;

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

        var thread = await _projectClient.Threads.GetThreadAsync(threadId);
        if (thread == null)
        {
            _logger.LogWarning("No persistent thread found for threadId: {ThreadId}", threadId);
            return new();
        }

        var messages = new List<ChatMessageHistory>();

        AsyncPageable<PersistentThreadMessage> threadMessages
       = _projectClient.Messages.GetMessagesAsync(
           threadId: thread.Value.Id, order: ListSortOrder.Ascending);

        IReadOnlyList<PersistentThreadMessage> messages1 = [.._projectClient.Messages.GetMessages(
        threadId: thread.Value.Id,   order: ListSortOrder.Ascending )];

        await foreach (PersistentThreadMessage threadMessage in threadMessages)
        {
            Console.Write($"{threadMessage.CreatedAt:yyyy-MM-dd HH:mm:ss} - {threadMessage.Role,10}: ");
            foreach (var contentItem in threadMessage.ContentItems)
            {
                if (contentItem is MessageTextContent textItem)
                {
                    Console.Write(textItem.Text);
                    messages.Add(new ChatMessageHistory
                    {
                        Role = threadMessage.Role == MessageRole.User ? "user" : "assistant",
                        Content = textItem.Text,
                        CreatedAt = threadMessage.CreatedAt.ToString("o")
                    });
                }
            }
        }

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

    public async Task<PersistentAgentThread> GetOrCreateAgentThreadAsync(string? threadId)
    {
        _logger.LogInformation("Getting or creating agent thread for threadId: {ThreadId}", threadId);
        if (!string.IsNullOrEmpty(threadId))
        {
            var persistentThread = await _projectClient.Threads.GetThreadAsync(threadId);
            if (persistentThread != null)
            {
                _logger.LogInformation("Found existing thread for threadId: {ThreadId}", threadId);
                return persistentThread;
            }
        }
        _logger.LogInformation("Creating new agent thread.");
        return await _projectClient.Threads.CreateThreadAsync();
    }

    private async Task<PersistentAgent> GetOrCreateAgentAsync(string agentName, string? agentId)
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

            var modelDeploymentName = "gpt-4o";
            var mcpServerUrl = Config.CONTOSO_STORE_MCP_URL;
            var mcpServerLabel = Config.CONTOSO_STORE_MCP_SERVER_LABEL;
            MCPToolDefinition mcpTool = new(mcpServerLabel, mcpServerUrl);

            agentDefinition = await _projectClient.Administration.CreateAgentAsync(
                modelId,
                name: agentName,
                instructions: agentInstructions,
                tools: [mcpTool]);
        }

        return agentDefinition;
    }

    private async Task InvokeAgent(string userPrompt, PersistentAgent agent, PersistentAgentThread thread, ILogger logger)
    {
        var mcpServerLabel = Config.CONTOSO_STORE_MCP_SERVER_LABEL;
        MCPToolResource mcpToolResource = new(mcpServerLabel);
        mcpToolResource.UpdateHeader("SuperSecret", "123456");
        ToolResources toolResources = mcpToolResource.ToToolResources();

        logger.LogInformation("Invoking agent for threadId: {ThreadId}", thread.Id);
        // Create message to thread
        var message = _projectClient.Messages.CreateMessage(
            thread.Id, MessageRole.User, userPrompt);

        ThreadRun run = _projectClient.Runs.CreateRun(thread, agent, toolResources);

        while (run.Status == RunStatus.Queued ||
            run.Status == RunStatus.InProgress ||
            run.Status == RunStatus.RequiresAction)
        {
            Thread.Sleep(TimeSpan.FromMilliseconds(1000));
            run = _projectClient.Runs.GetRun(thread.Id, run.Id);

            if (run.Status == RunStatus.RequiresAction &&
                run.RequiredAction is SubmitToolApprovalAction toolApprovalAction)
            {
                var toolApprovals = new List<ToolApproval>();
                foreach (var toolCall in toolApprovalAction.SubmitToolApproval.ToolCalls)
                {
                    if (toolCall is RequiredMcpToolCall mcpToolCall)
                    {
                        Console.WriteLine($"Approving MCP tool call: {mcpToolCall.Name}, Arguments: {mcpToolCall.Arguments}");
                        toolApprovals.Add(new ToolApproval(mcpToolCall.Id, approve: true)
                        {
                            Headers = { ["SuperSecret"] = "123456" }
                        });
                    }
                }

                if (toolApprovals.Count > 0)
                {
                    run = _projectClient.Runs.SubmitToolOutputsToRun(thread.Id, run.Id, toolApprovals: toolApprovals);
                }
            }
        }
    }
}
