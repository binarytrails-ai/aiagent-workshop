using Microsoft.SemanticKernel.Agents.AzureAI;
using AIAgent.API.Models;

namespace AIAgent.API.Services
{
    /// <summary>
    /// Service interface for Azure AI Agent operations.
    /// </summary>
    public interface IAzureAIAgentService
    {
        Task<SendMessageResult> SendMessage(string userPrompt, string agentName, string agentId, string threadId);
        Task<List<ChatMessageHistory>> GetChatMessageHistoryAsync(string threadId);
        Task<AzureAIAgentThread> GetOrCreateAgentThreadAsync(string? threadId);
    }
}
