using Microsoft.AspNetCore.Mvc;
using AIAgent.API.Models;

namespace AIAgent.API.Controllers
{
    [ApiController]
    [Route("api/input_task")]
    public class InputTaskController : ControllerBase
    {
        /// <summary>
        /// Accepts a new input task from the user and starts the planning process.
        /// </summary>
        /// <param name="inputTask">The input task details.</param>
        /// <returns>Status of task reception.</returns>
        [HttpPost]
        public IActionResult InputTask([FromBody] InputTaskDto inputTask)
        {
            // TODO: Implement logic to process the input task and start planning
            return Ok(new { status = "Task received" });
        }
    }
}
