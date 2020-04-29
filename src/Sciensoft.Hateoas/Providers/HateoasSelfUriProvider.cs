using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Sciensoft.Hateoas.Repositories;
using System.Linq;

namespace Sciensoft.Hateoas.Providers
{
	internal class HateoasSelfUriProvider : HateoasUriProvider<InMemoryPolicyRepository.SelfPolicy>
	{
		public HateoasSelfUriProvider(IHttpContextAccessor contextAccessor, LinkGenerator linkGenerator)
			: base(contextAccessor, linkGenerator)
		{ }

		public override (string Method, string Uri) GenerateEndpoint(InMemoryPolicyRepository.SelfPolicy policy, object result)
		{
			var request = HttpContext.Request;
			var routeData = HttpContext.GetRouteData();

			string rawResult = result.ToString();
			string formatedResult = GetFormatedPath(rawResult);

			if (routeData.Values.Any(r => r.Value.Equals(formatedResult)))
			{
				string virtualPath = LinkGenerator.GetPathByRouteValues(HttpContext, null, routeData.Values);
				string finalVirtualPath = GetFormatedPath(virtualPath);
				return (policy.Method, $"{Host}/{finalVirtualPath}");
			}

			// TODO : Improve link generation
			string t = GetFormatedPath(policy.Template);
			string r = GetFormatedPath(result.ToString());

			var path = GetFormatedPath($"{request.Path}");
			var finalPath = GetFormatedPath($"{t}/{r}");

			return (policy.Method, $"{Host}/{path}/{finalPath}");
		}
	}
}
