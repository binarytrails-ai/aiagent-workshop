namespace AIAgent.API.Models
{
    public class ChatRequest
    {
        public string AgentName { get; set; }
        public string? AgentId { get; set; }
        public string? ThreadId { get; set; }
        public string Message { get; set; }
    }
}
