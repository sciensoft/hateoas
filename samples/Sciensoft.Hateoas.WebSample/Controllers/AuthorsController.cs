using Microsoft.AspNetCore.Mvc;
using Sciensoft.Hateoas.WebSample.Models;
using Sciensoft.Hateoas.WebSample.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sciensoft.Hateoas.WebSample.Controllers
{
	public class AuthorsController : Controller
	{
		public const string UpdateAuthorById = nameof(UpdateAuthorById);

		[HttpGet]
		public ActionResult<IEnumerable<ArticleViewModel>> List()
			=> Ok(InMemoryAuthorCollection.Authors.Select(b => b.Value));

		[HttpGet]
		public IEnumerable<AuthorViewModel> Get()
			=> InMemoryAuthorCollection.Authors.Select(a => a.Value);

		[HttpGet]
		public AuthorViewModel GetById(Guid id)
			=> InMemoryAuthorCollection.Authors.FirstOrDefault(a => a.Key.Equals(id)).Value;

		[HttpPut(Name = UpdateAuthorById)]
		public IActionResult Put(Guid id, [FromBody] AuthorViewModel author)
		{
			Debug.Assert(author != null);

			author.Id = id;
			var model = InMemoryArticleCollection.Articles.FirstOrDefault(x => x.Key.Equals(id));

			if (model.Value == null)
			{
				return NotFound(id);
			}

			if (!InMemoryAuthorCollection.Authors.TryAdd(id, author))
			{
				InMemoryAuthorCollection.Authors[id] = author;
			}

			return CreatedAtAction(nameof(Get), id);
		}

	}
}
