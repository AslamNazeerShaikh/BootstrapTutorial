using Microsoft.AspNetCore.Mvc;

namespace BootstrapTutorial.WebUi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class NewApiController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Hello from NewApiController.");
        }
    }
}
