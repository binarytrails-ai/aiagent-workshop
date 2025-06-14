using Microsoft.AspNetCore.Mvc;

namespace AIAgent.API.Controllers
{
    [ApiController]
    [Route("api/agent-tools")]
    public class AgentToolsController : ControllerBase
    {
        /// <summary>
        /// Retrieves the list of available agent tools for the system or user.
        /// </summary>
        /// <returns>List of agent tools.</returns>
        [HttpGet]
        public ActionResult<List<object>> GetAgentTools()
        {
            // TODO: Replace object with a proper AgentToolDto and implement logic to fetch agent tools
            return Ok(new List<object>()); // Returns an empty list as a placeholder
        }
    }
}
