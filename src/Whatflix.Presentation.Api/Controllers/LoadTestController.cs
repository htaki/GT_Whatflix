using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Text;

namespace Whatflix.Presentation.Api.Controllers
{
    [Route("")]
    [ApiController]
    public class LoadTestController : ControllerBase
    {
        [HttpGet("loaderio-0923e5d671ce1d0319577d94252c2edf.txt")]
        public IActionResult Get()
        {
            var file = "loaderio-0923e5d671ce1d0319577d94252c2edf";
            return File(Encoding.ASCII.GetBytes(file), MediaTypeNames.Application.Octet, "loaderio-0923e5d671ce1d0319577d94252c2edf.txt");
        }
    }
}