using Microsoft.AspNetCore.Http;
using Sciensoft.Hateoas.Repository;
using System;
using System.Text.RegularExpressions;

namespace Sciensoft.Hateoas.Abstractions
{
	internal abstract class HateoasUriProvider<TPolicy>
		where TPolicy : PolicyInMemoryRepository.Policy
	{
		protected readonly IHttpContextAccessor ContextAccessor;

		protected HateoasUriProvider(IHttpContextAccessor contextAccessor)
		{
			ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
		}

		public abstract (string Method, string Uri) GenerateEndpoint(TPolicy policy, object result);

		protected string GetFormatedPath(string path)
		{
			if (path == null)
			{
				return null;
			}

			if (path.StartsWith("/"))
			{
				var startReplaceRegex = new Regex("^(/)*");
				path = startReplaceRegex.Replace(path, string.Empty);
			}

			if (path.EndsWith("/"))
			{
				var endReplaceRegex = new Regex("(/)$");
				path = endReplaceRegex.Replace(path, string.Empty);
			}

			return path.ToLower();
		}

		protected void AssureIsNotNull(object @object, string name)
		{
			if (@object == null)
			{
				throw new ArgumentNullException(name);
			}
		}
	}
}
