using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AIAgent.API.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessagesController : ControllerBase
    {
        /// <summary>
        /// Retrieves all messages for the current user/session.
        /// </summary>
        /// <returns>List of messages.</returns>
        [HttpGet]
        public ActionResult<List<object>> GetMessages()
        {
            // TODO: Replace object with a proper MessageDto and implement logic to fetch messages
            return Ok(new List<object>()); // Returns an empty list as a placeholder
        }

        /// <summary>
        /// Deletes all messages for the current user/session.
        /// </summary>
        /// <returns>No content.</returns>
        [HttpDelete]
        public IActionResult DeleteMessages()
        {
            // TODO: Implement logic to delete messages
            return NoContent();
        }
    }
}
