using Microsoft.AspNetCore.Mvc;
using AIAgent.API.Models;

namespace AIAgent.API.Controllers
{
    [ApiController]
    [Route("api")]
    public class AgentController : ControllerBase
    {
        // GET /api/agent_messages/{session_id}
        [HttpGet("agent_messages/{session_id}")]
        public IActionResult GetAgentMessages(string session_id)
        {
            return Ok(new List<AgentMessageDto>()); // TODO: Return real data
        }

        //// DELETE /api/messages
        //[HttpDelete("messages")]
        //public IActionResult DeleteMessages()
        //{
        //    return NoContent();
        //}

        //// GET /api/messages
        //[HttpGet("messages")]
        //public IActionResult GetMessages()
        //{
        //    return Ok(new object[0]); // TODO: Return List<MessageDto>
        //}
    }
}
