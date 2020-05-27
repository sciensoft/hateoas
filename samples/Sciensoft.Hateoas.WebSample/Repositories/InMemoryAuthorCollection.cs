using Sciensoft.Hateoas.WebSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sciensoft.Hateoas.WebSample.Repositories
{
	internal static class InMemoryAuthorCollection
	{
		public static readonly IDictionary<Guid, AuthorViewModel> Authors = (new List<AuthorViewModel> {
			new AuthorViewModel {
				Id = Guid.Parse("{41AC1BE0-7F58-4593-9FC7-F6E646924404}"),
				FirstName = "Andrew",
				LastName = "Mayne"
			},
			new AuthorViewModel {
				Id = Guid.Parse("{B5E319CF-06C1-42A4-A67D-DA194D7414E7}"),
				FirstName = "Christopher",
				LastName = "Greyson"
			},
			new AuthorViewModel {
				Id = Guid.Parse("{EC48F568-8B2E-4DA0-99F9-D217EB58374D}"),
				FirstName = "Marin",
				LastName = "Montgomery"
			},
			new AuthorViewModel {
				Id = Guid.Parse("{70154BCD-01A0-44CF-A450-0AAEE2231B05}"),
				FirstName = "Sonja",
				LastName = "Yoerg"
			},
			new AuthorViewModel {
				Id = Guid.Parse("{B6FD0FA9-E122-4E60-A790-D59FD18A7FC6}"),
				FirstName = "Delia",
				LastName = "Owens"
			},
			new AuthorViewModel {
				Id = Guid.Parse("{0D47C395-A956-46A5-8886-478ABB339DD2}"),
				FirstName = "James",
				LastName = "Maxwell"
			}
		}).ToDictionary(k => k.Id);
	}
}
