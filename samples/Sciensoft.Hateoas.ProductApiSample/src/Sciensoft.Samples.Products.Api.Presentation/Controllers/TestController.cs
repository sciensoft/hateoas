using Microsoft.AspNetCore.Mvc;
using Sciensoft.Samples.Products.AspNetCore.Attributes;

namespace Sciensoft.Samples.Products.Api.Presentation.Controllers
{
    [ApiController]
    [Route("api/tests")]
    [ContentNegotiationController]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Get(int i)
        {
            return Ok(i);
        }

        [HttpGet]
        public IActionResult Get(string s)
        {
            return Ok(s);
        }

        [HttpGet]
        public IActionResult Get(decimal d)
        {
            return Ok(d);
        }
    }
}
