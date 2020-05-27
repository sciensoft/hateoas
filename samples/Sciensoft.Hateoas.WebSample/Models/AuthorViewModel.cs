using System;
using System.Collections.Generic;

namespace Sciensoft.Hateoas.WebSample.Models
{
	public class AuthorViewModel
	{
		public Guid Id { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public IList<BookViewModel> Books { get; set; }
	}
}
