using Microsoft.AspNetCore.Mvc;

namespace Whatflix.Api._01_Presentation.Controllers
{
    [Route("movies")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        [HttpGet("user/{userId}/search")]
        public IActionResult Get(int userId, string text)
        {
            return Ok();
        }

        [HttpGet("users")]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}