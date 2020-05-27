using System;
using System.Collections.Generic;

namespace Sciensoft.Hateoas.WebSample.Models
{
	public class BookViewModel
	{
		public Guid Id { get; set; }

		public string Title { get; set; }

		public string Author { get; set; }

		public ICollection<object> Tags { get; set; }

		public object Reference { get; set; }
	}
}
