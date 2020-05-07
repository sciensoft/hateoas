using Microsoft.AspNetCore.Mvc;
using Sciensoft.Hateoas.WebSample.Models;
using Sciensoft.Hateoas.WebSample.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Sciensoft.Hateoas.WebSample.Controllers
{
	public class TagsController : ControllerBase
	{
		[HttpGet]
		public ActionResult<IEnumerable<TagViewModel>> List()
			=> Ok(InMemoryTagCollection.Tags.Select(b => b.Value));

		public ActionResult<TagViewModel> Get(Guid id)
		{
			var model = InMemoryTagCollection.Tags.FirstOrDefault(x => x.Key.Equals(id));

			if (model.Value == null)
			{
				return NotFound(id);
			}

			return model.Value;
		}

		[HttpPost]
		public IActionResult Post([FromBody] TagViewModel article)
		{
			Debug.Assert(article != null);

			if (!InMemoryTagCollection.Tags.TryAdd(article.Id, article))
			{
				throw new InvalidOperationException($"Article with Id '{article.Id}' already exists. Try PUT operation to update the item.");
			}

			return CreatedAtAction(nameof(Get), article.Id);
		}

		public IActionResult Put(Guid id, [FromBody] TagViewModel article)
		{
			Debug.Assert(article != null);

			article.Id = id;
			var model = InMemoryTagCollection.Tags.FirstOrDefault(x => x.Key.Equals(id));

			if (model.Value == null)
			{
				return NotFound(id);
			}

			if (!InMemoryTagCollection.Tags.TryAdd(id, article))
			{
				InMemoryTagCollection.Tags[id] = article;
			}

			return CreatedAtAction(nameof(Get), id);
		}

		public IActionResult Delte(Guid id)
		{
			var model = InMemoryTagCollection.Tags.FirstOrDefault(x => x.Key.Equals(id));

			if (model.Value == null)
			{
				return NotFound();
			}

			InMemoryTagCollection.Tags.Remove(id);

			return Ok();
		}
	}
}
