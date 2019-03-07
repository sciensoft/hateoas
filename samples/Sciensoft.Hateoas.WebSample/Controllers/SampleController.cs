using Microsoft.AspNetCore.Mvc;
using Sciensoft.Hateoas.WebSample.Models;
using System;

namespace Sciensoft.Hateoas.WebSample.Controllers
{
	[Route("api/samples")]
	public class SampleController : Controller
	{
		public const string GetWithId = nameof(GetWithId);

		[HttpGet]
		public ActionResult<SampleViewModel> Get()
		{
			string location = Url.Action(nameof(Get), new { id = Guid.NewGuid() });

			return Ok(new SampleViewModel
			{
				Id = Guid.NewGuid(),
				Name = "Hello Sample View",
				Samples = new[] { "A", "B", "C" }
			});
		}

		[HttpPost(Name = GetWithId)]
		public ActionResult<SampleViewModel> Post(Guid id)
		{
			return Ok(new SampleViewModel
			{
				Id = id,
				Name = "Hello Sample View",
				Samples = new[] { "A", "B", "C" }
			});
		}
	}
}
