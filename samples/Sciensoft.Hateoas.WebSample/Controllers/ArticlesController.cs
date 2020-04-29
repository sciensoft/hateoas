using Microsoft.AspNetCore.Mvc;
using Sciensoft.Hateoas.WebSample.Models;
using Sciensoft.Hateoas.WebSample.Repositories;
using System;
using System.Linq;

namespace Sciensoft.Hateoas.WebSample.Controllers
{
	[Route("api/[controller]/[action]")]
	public class ArticlesController : ControllerBase
	{
		public const string UpdatedWithId = nameof(UpdatedWithId);

		[HttpGet]
		public ActionResult<ArticleViewModel> List()
		{
			return Ok(InMemoryArticlesRepository.Articles);
		}

		[HttpGet("{id:guid}")]
		public ActionResult<ArticleViewModel> Get(Guid id)
		{
			var model = InMemoryArticlesRepository.Articles.FirstOrDefault(x => x.Id.Equals(id));

			if (model == null)
			{
				return NotFound();
			}

			return model;
		}

		[HttpPost("{id:guid}", Name = UpdatedWithId)]
		public IActionResult Update(Guid id, ArticleViewModel article)
		{
			var model = InMemoryArticlesRepository.Articles.FirstOrDefault(x => x.Id.Equals(id));

			if (model == null)
			{
				InMemoryArticlesRepository.Articles.Add(article);
			}

			model = article;

			return Ok();
		}
	}
}
