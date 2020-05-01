using Sciensoft.Hateoas.WebSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sciensoft.Hateoas.WebSample.Repositories
{
	internal static class InMemoryTagCollection
	{
		public static readonly IDictionary<Guid, TagViewModel> Tags = (new List<TagViewModel>
		{
			new TagViewModel
			{
				Id = Guid.Parse("{9f4c57ea-8bc8-11ea-9420-fbb231d16d0c}"),
				Name = "Fiction"
			},
			new TagViewModel
			{
				Id = Guid.Parse("{9f4c5948-8bc8-11ea-9421-23f6b63ef773}"),
				Name = "Crime"
			},
			new TagViewModel
			{
				Id = Guid.Parse("{9f4c597a-8bc8-11ea-9422-2fbb559654e4}"),
				Name = "Thriller"
			},
			new TagViewModel
			{
				Id = Guid.Parse("{9f4c59a2-8bc8-11ea-9423-b79c73709311}"),
				Name = "Murder"
			},
			new TagViewModel
			{
				Id = Guid.Parse("{9f4c59ca-8bc8-11ea-9424-f3d24bfb9bc8}"),
				Name = "Suspense"
			},
			new TagViewModel
			{
				Id = Guid.Parse("{9f4c59f2-8bc8-11ea-9425-47873b40b460}"),
				Name = "Literature"
			},
			new TagViewModel
			{
				Id = Guid.Parse("{9f4c5a24-8bc8-11ea-9426-3362766ce4a9}"),
				Name = "Women"
			},
			new TagViewModel
			{
				Id = Guid.Parse("{9f4c5a4c-8bc8-11ea-9427-273cd9d7ec1b}"),
				Name = "Mothers"
			},
			new TagViewModel
			{
				Id = Guid.Parse("{9f4c5a74-8bc8-11ea-9428-171ca1b454b2}"),
				Name = "Fantasy"
			},
			new TagViewModel
			{
				Id = Guid.Parse("{9f4c5a9c-8bc8-11ea-9429-37075559ced2}"),
				Name = "Adventure"
			},
			new TagViewModel
			{
				Id = Guid.Parse("{9f4c5ac4-8bc8-11ea-942a-37d5e00275d7}"),
				Name = "Action"
			}
		}).ToDictionary(b => b.Id);
	}
}
