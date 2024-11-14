using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HelloWorldController : ControllerBase
    {
        // GET: /api/helloworld
        [HttpGet]
        public IActionResult GetHelloWorld()
        {
            return Ok("Hello World!");
        }
    }
}
