using Microsoft.AspNetCore.Mvc;
using Sciensoft.Hateoas.WebSample.Models;
using Sciensoft.Hateoas.WebSample.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sciensoft.Hateoas.WebSample.Controllers
{
	[Route("api/books")]
	public class BookController : ControllerBase
	{
		public const string UpdateWithId = nameof(UpdateWithId);
		public const string DeleteWithId = nameof(DeleteWithId);

		[HttpGet]
		public ActionResult<IEnumerable<BookViewModel>> Get()
			=> Ok(InMemoryBookRepository.Books.Select(b => b.Value));

		[HttpGet("{id:guid}")]
		public ActionResult<BookViewModel> Get(Guid id)
		{
			var model = InMemoryBookRepository.Books.FirstOrDefault(x => x.Key.Equals(id));

			if (model.Value == null)
			{
				return NotFound(id);
			}

			return model.Value;
		}

		[HttpPost]
		public IActionResult Post([FromBody] BookViewModel book)
		{
			Debug.Assert(book != null);

			if (!InMemoryBookRepository.Books.TryAdd(book.Id, book))
			{
				throw new InvalidOperationException($"Book with Id '{book.Id}' already exists. Try PUT operation to update the item.");
			}

			return CreatedAtAction(nameof(Get), book.Id);
		}

		[HttpPut("{id:guid}", Name = UpdateWithId)]
		public IActionResult Put(Guid id, [FromBody] BookViewModel book)
		{
			Debug.Assert(book != null);

			book.Id = id;
			var model = InMemoryBookRepository.Books.FirstOrDefault(x => x.Key.Equals(id));

			if (model.Value == null)
			{
				return NotFound(id);
			}

			if (!InMemoryBookRepository.Books.TryAdd(id, book))
			{
				InMemoryBookRepository.Books[id] = book;
			}

			return CreatedAtAction(nameof(Get), id);
		}

		[HttpDelete("{id:guid}", Name = DeleteWithId)]
		public IActionResult Delte(Guid id)
		{
			var model = InMemoryBookRepository.Books.FirstOrDefault(x => x.Key.Equals(id));

			if (model.Value == null)
			{
				return NotFound();
			}

			InMemoryBookRepository.Books.Remove(id);

			return Ok();
		}
	}
}
