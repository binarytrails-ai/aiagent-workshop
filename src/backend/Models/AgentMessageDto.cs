namespace AIAgent.API.Models
{
    public class AgentMessageDto
    {
        public string Id { get; set; }
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public string PlanId { get; set; }
        public string Content { get; set; }
        public string Source { get; set; }
        public string StepId { get; set; }
    }
}
