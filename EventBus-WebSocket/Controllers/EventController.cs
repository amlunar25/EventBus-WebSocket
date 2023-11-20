using EventBus_WebSocket.EventBusHandler; // Adjust the namespace as needed
using Microsoft.AspNetCore.Mvc;
using System;

namespace EventBus_WebSocket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly EventBus _eventBus;

        public EventController(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        [HttpPost("publish")]
        public IActionResult Publish([FromBody] MessageEventArgs message)
        {
            try
            {
                _eventBus.Publish(message.Message);
                return Ok("Message published successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error publishing message: {ex.Message}");
            }
        }
    }
}
