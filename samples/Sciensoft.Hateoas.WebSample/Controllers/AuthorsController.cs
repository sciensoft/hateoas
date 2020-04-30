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
		public IEnumerable<AuthorViewModel> Get()
			=> InMemoryAuthorsCollection.Authors.Select(a => a.Value);

		[HttpGet]
		public AuthorViewModel GetById(Guid id)
			=> InMemoryAuthorsCollection.Authors.FirstOrDefault(a => a.Key.Equals(id)).Value;

		[HttpPut(Name = UpdateAuthorById)]
		public IActionResult Put(Guid id, [FromBody] AuthorViewModel author)
		{
			Debug.Assert(author != null);

			author.Id = id;
			var model = InMemoryArticlesCollection.Articles.FirstOrDefault(x => x.Key.Equals(id));

			if (model.Value == null)
			{
				return NotFound(id);
			}

			if (!InMemoryAuthorsCollection.Authors.TryAdd(id, author))
			{
				InMemoryAuthorsCollection.Authors[id] = author;
			}

			return CreatedAtAction(nameof(Get), id);
		}

	}
}
