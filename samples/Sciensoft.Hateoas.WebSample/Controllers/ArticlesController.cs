using Microsoft.AspNetCore.Mvc;
using Sciensoft.Hateoas.WebSample.Models;
using Sciensoft.Hateoas.WebSample.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sciensoft.Hateoas.WebSample.Controllers
{
	[Route("api/[controller]/[action]")]
	public class ArticlesController : ControllerBase
	{
		public const string UpdateWithId = nameof(UpdateWithId);
		public const string DeleteWithId = nameof(DeleteWithId);

		[HttpGet]
		public ActionResult<IEnumerable<ArticleViewModel>> Get()
			=> Ok(InMemoryArticlesRepository.Articles.Select(b => b.Value));

		[HttpGet("{id:guid}")]
		public ActionResult<ArticleViewModel> Get(Guid id)
		{
			var model = InMemoryArticlesRepository.Articles.FirstOrDefault(x => x.Key.Equals(id));

			if (model.Value == null)
			{
				return NotFound(id);
			}

			return model.Value;
		}

		[HttpPost]
		public IActionResult Post([FromBody] ArticleViewModel article)
		{
			Debug.Assert(article != null);

			if (!InMemoryArticlesRepository.Articles.TryAdd(article.Id, article))
			{
				throw new InvalidOperationException($"Article with Id '{article.Id}' already exists. Try PUT operation to update the item.");
			}

			return CreatedAtAction(nameof(Get), article.Id);
		}

		[HttpPut("{id:guid}", Name = UpdateWithId)]
		public IActionResult Put(Guid id, [FromBody] ArticleViewModel article)
		{
			Debug.Assert(article != null);

			article.Id = id;
			var model = InMemoryArticlesRepository.Articles.FirstOrDefault(x => x.Key.Equals(id));

			if (model.Value == null)
			{
				return NotFound(id);
			}

			if (!InMemoryArticlesRepository.Articles.TryAdd(id, article))
			{
				InMemoryArticlesRepository.Articles[id] = article;
			}

			return CreatedAtAction(nameof(Get), id);
		}

		[HttpDelete("{id:guid}", Name = DeleteWithId)]
		public IActionResult Delte(Guid id)
		{
			var model = InMemoryArticlesRepository.Articles.FirstOrDefault(x => x.Key.Equals(id));

			if (model.Value == null)
			{
				return NotFound();
			}

			InMemoryArticlesRepository.Articles.Remove(id);

			return Ok();
		}
	}
}
