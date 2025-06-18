using AIAgent.API.Agents;
using AIAgent.API.Models;
using AIAgent.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIAgent.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class ChatController : ControllerBase
    {
        private readonly IAzureAIAgentService _azureAIAgentService;
        private readonly TechSupportAgentConfig _techSupportAgentConfig = new();
        private readonly ILogger<ChatController> _logger;

        public ChatController(IAzureAIAgentService azureAIAgentService, ILogger<ChatController> logger)
        {
            _azureAIAgentService = azureAIAgentService;
            _logger = logger;
        }

        /// <summary>
        /// Gets information about the chat agent.
        /// </summary>
        // GET /agent
        [HttpGet("agent")]
        public IActionResult GetChatAgent()
        {
            _logger.LogInformation("Fetching chat agent info.");
            var agentInfo = new
            {
                Name = _techSupportAgentConfig.AgentName,
                DisplayName = _techSupportAgentConfig.GetAgentDisplayName(),
                Description = _techSupportAgentConfig.GetDescription(),
            };
            return Ok(agentInfo);
        }

        /// <summary>
        /// Gets the chat message history for a given thread.
        /// </summary>
        // GET /chat/history
        [HttpGet("chat/history")]
        public async Task<IActionResult> History([FromQuery] string? agentId, [FromQuery] string? threadId)
        {
            _logger.LogInformation("Fetching chat history for threadId: {ThreadId}", threadId);
            try
            {
                var messages = await _azureAIAgentService.GetChatMessageHistoryAsync(threadId).ConfigureAwait(false);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching chat history for threadId: {ThreadId}", threadId);
                return StatusCode(500, "An error occurred while fetching chat history.");
            }
        }

        /// <summary>
        /// Sends a chat message and returns the agent thread id.
        /// </summary>
        [HttpPost("chat/send")]
        public async Task<IActionResult> ChatSend([FromBody] ChatRequest request)
        {
            _logger.LogInformation("Sending chat message for agentId: {AgentId}, threadId: {ThreadId}", request.AgentId, request.ThreadId);
            try
            {
                var result = await _azureAIAgentService.SendMessage
                    (request.Message, request.AgentName, request.AgentId, request.ThreadId);

                var response = new ChatCompletionResponse
                {
                    AgentId = result.AgentId,
                    ThreadId = result.ThreadId
                };

                _logger.LogInformation("Message sent successfully. agentId: {AgentId}, threadId: {ThreadId}", result.AgentId, result.ThreadId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending chat message for agentId: {AgentId}, threadId: {ThreadId}", request.AgentId, request.ThreadId);
                return StatusCode(500, "An error occurred while sending the chat message.");
            }
        }
    }
}
