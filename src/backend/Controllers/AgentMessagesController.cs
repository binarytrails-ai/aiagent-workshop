using Microsoft.AspNetCore.Mvc;
using AIAgent.API.Models;

namespace AIAgent.API.Controllers
{
    [ApiController]
    [Route("api/agent_messages")]
    public class AgentMessagesController : ControllerBase
    {
        /// <summary>
        /// Retrieves all agent messages for a given session.
        /// </summary>
        /// <param name="session_id">The session ID for which to retrieve agent messages.</param>
        /// <returns>List of agent messages for the specified session.</returns>
        [HttpGet("{session_id}")]
        public ActionResult<List<AgentMessageDto>> GetAgentMessages(string session_id)
        {
            // TODO: Implement logic to fetch agent messages for the given session_id
            return Ok(new List<AgentMessageDto>()); // Returns an empty list as a placeholder
        }
    }
}
