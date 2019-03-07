using System;
using System.Collections.Generic;

namespace Sciensoft.Hateoas.WebSample.Models
{
	public class SampleViewModel
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public ICollection<object> Samples { get; set; }

		public object Null { get; set; }
	}
}
