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
		public const string UpdateArticleById = nameof(UpdateArticleById);
		public const string DeleteArticleById = nameof(DeleteArticleById);

		[HttpGet]
		public ActionResult<IEnumerable<ArticleViewModel>> List()
			=> Ok(InMemoryArticleCollection.Articles.Select(b => b.Value));

		[HttpGet("{id:guid}")]
		public ActionResult<ArticleViewModel> Get(Guid id)
		{
			var model = InMemoryArticleCollection.Articles.FirstOrDefault(x => x.Key.Equals(id));

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

			if (!InMemoryArticleCollection.Articles.TryAdd(article.Id, article))
			{
				throw new InvalidOperationException($"Article with Id '{article.Id}' already exists. Try PUT operation to update the item.");
			}

			return CreatedAtAction(nameof(Get), article.Id);
		}

		[HttpPut("{id:guid}", Name = UpdateArticleById)]
		public IActionResult Put(Guid id, [FromBody] ArticleViewModel article)
		{
			Debug.Assert(article != null);

			article.Id = id;
			var model = InMemoryArticleCollection.Articles.FirstOrDefault(x => x.Key.Equals(id));

			if (model.Value == null)
			{
				return NotFound(id);
			}

			if (!InMemoryArticleCollection.Articles.TryAdd(id, article))
			{
				InMemoryArticleCollection.Articles[id] = article;
			}

			return CreatedAtAction(nameof(Get), id);
		}

		[HttpDelete("{id:guid}", Name = DeleteArticleById)]
		public IActionResult Delte(Guid id)
		{
			var model = InMemoryArticleCollection.Articles.FirstOrDefault(x => x.Key.Equals(id));

			if (model.Value == null)
			{
				return NotFound();
			}

			InMemoryArticleCollection.Articles.Remove(id);

			return Ok();
		}
	}
}
