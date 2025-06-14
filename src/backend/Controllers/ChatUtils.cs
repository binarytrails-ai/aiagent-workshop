using AIAgent.API.Models;
using Azure.AI.Agents.Persistent;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.AzureAI;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AIAgent.API.Controllers
{
    public static class ChatUtils
    {
        public static async Task<List<ChatMessageHistory>> GetChatMessageHistoryAsync(string threadId, PersistentAgentsClient projectClient)
        {
            var persistentThread = await projectClient.Threads.GetThreadAsync(threadId);
            if (persistentThread == null)
                return new List<ChatMessageHistory>(); // No history for this thread

            var agentThread = new AzureAIAgentThread(projectClient, persistentThread.Value.Id);

            var messages = new List<ChatMessageHistory>();
            await foreach (var msg in projectClient.Messages.GetMessagesAsync(agentThread.Id))
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
            return messages;
        }

        public static async Task InvokeAgent(string userPrompt, AzureAIAgent agent, AzureAIAgentThread agentThread)
        {
            var userMessage = new ChatMessageContent(AuthorRole.User, userPrompt);
            await foreach (var response in agent.InvokeAsync(userMessage, agentThread))
            {
            }
        }

        public static async Task<AzureAIAgentThread> GetOrCreateAgentThreadAsync(string? threadId, PersistentAgentsClient projectClient)
        {
            AzureAIAgentThread agentThread = null;
            if (!string.IsNullOrEmpty(threadId))
            {
                var persistentThread = await projectClient.Threads.GetThreadAsync(threadId);
                if (persistentThread != null)
                    agentThread = new AzureAIAgentThread(projectClient, persistentThread.Value.Id);
            }

            agentThread ??= new AzureAIAgentThread(projectClient);
            return agentThread;
        }
    }
}
