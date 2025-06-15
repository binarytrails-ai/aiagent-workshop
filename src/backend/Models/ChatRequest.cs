namespace AIAgent.API.Models
{
    public class ChatRequest
    {
        public string? AgentId { get; set; }
        public string? ThreadId { get; set; }
        public string Message { get; set; }
    }
}
