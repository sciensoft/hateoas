using Microsoft.AspNetCore.Mvc;

namespace Sciensoft.Hateoas.WebSample.Controllers
{
	[Route("api/samples/inline/resource")]
	public class NoSampleController : Controller
	{
		[HttpGet]
		public IActionResult Get()
		{
			return Ok(new
			{
				Identifier = 1,
				Message = "Hello World",
			});
		}
	}
}
