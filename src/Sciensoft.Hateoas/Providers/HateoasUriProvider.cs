using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Sciensoft.Hateoas.Repositories;
using System;
using System.Text.RegularExpressions;

namespace Sciensoft.Hateoas.Providers
{
	internal abstract class HateoasUriProvider<TPolicy>
		where TPolicy : InMemoryPolicyRepository.Policy
	{
		protected readonly IHttpContextAccessor ContextAccessor;
		protected readonly LinkGenerator LinkGenerator;

		protected HateoasUriProvider(IHttpContextAccessor contextAccessor, LinkGenerator linkGenerator)
		{
			ContextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
			LinkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
		}

		protected HttpContext HttpContext => ContextAccessor.HttpContext;

		protected object EndpointDataSource => HttpContext.RequestServices.GetRequiredService<EndpointDataSource>();

		protected string Host
		{
			get
			{
				var context = ContextAccessor.HttpContext;
				var request = context.Request;

				return GetFormatedPath($"{request.Scheme}://{request.Host}");
			}
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
