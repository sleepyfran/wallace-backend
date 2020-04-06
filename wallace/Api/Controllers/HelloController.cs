using Microsoft.AspNetCore.Mvc;

namespace Wallace.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HelloController : ControllerBase
    {
        public HelloController() { }

        [HttpGet]
        public string Get()
        {
            return "Hi there!";
        }
    }
}
