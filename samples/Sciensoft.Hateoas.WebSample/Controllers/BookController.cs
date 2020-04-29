using Microsoft.AspNetCore.Mvc;
using Sciensoft.Hateoas.WebSample.Models;
using Sciensoft.Hateoas.WebSample.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sciensoft.Hateoas.WebSample.Controllers
{
	[Route("api/books")]
	public class BookController : ControllerBase
	{
		public const string PostWithId = nameof(PostWithId);
		public const string DeleteWithId = nameof(DeleteWithId);

		[HttpGet]
		public ActionResult<IEnumerable<BookViewModel>> Get()
		{
			string location = Url.Action(nameof(Get), new { id = Guid.NewGuid() });

			return Ok(InMemoryBookRepository.Books);
		}

		[HttpGet("{id:guid}")]
		public ActionResult<BookViewModel> Get(Guid id)
		{
			var model = InMemoryBookRepository.Books.FirstOrDefault(x => x.Id.Equals(id));

			if (model == null)
			{
				return NotFound(id);
			}

			return model;
		}

		[HttpPost("{id:guid}", Name = PostWithId)]
		public IActionResult Post(Guid id, BookViewModel book)
		{
			var model = InMemoryBookRepository.Books.FirstOrDefault(x => x.Id.Equals(id));

			if (model == null)
			{
				InMemoryBookRepository.Books.Add(book);
			}
			else
			{
				model = book;
			}

			return CreatedAtAction(nameof(Get), model.Id);
		}

		[HttpDelete("{id:guid}", Name = DeleteWithId)]
		public IActionResult Delte(Guid id)
		{
			var model = InMemoryBookRepository.Books.FirstOrDefault(x => x.Id.Equals(id));

			if (model == null)
			{
				return NotFound();
			}

			InMemoryBookRepository.Books.Remove(model);

			return Ok();
		}
	}
}
