using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Sciensoft.Hateoas.Repositories;
using System;
using System.Linq;

namespace Sciensoft.Hateoas.Providers
{
	internal class HateoasExternalUriProvider : HateoasUriProvider<InMemoryPolicyRepository.ExternalPolicy>
	{
		public HateoasExternalUriProvider(IHttpContextAccessor contextAccessor, LinkGenerator linkGenerator)
			: base(contextAccessor, linkGenerator)
		{ }

		public override (string Method, string Uri) GenerateEndpoint(InMemoryPolicyRepository.ExternalPolicy policy, object result)
		{
			var uris = policy.Hosts
				.Select(h => new Uri(h, UriKind.RelativeOrAbsolute))
				.Where(u => u.IsAbsoluteUri)
				.Select(u => new Uri(u, result.ToString()));

			return (policy.Method, string.Join(", ", uris));
		}
	}
}
