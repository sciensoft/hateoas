using Microsoft.AspNetCore.Http;
using Sciensoft.Hateoas.Abstractions;
using Sciensoft.Hateoas.Repository;

namespace Sciensoft.Hateoas.Providers
{
	internal class HateoasTemplateUriProvider : HateoasUriProvider<PolicyInMemoryRepository.TemplatePolicy>
	{
		protected HateoasTemplateUriProvider(IHttpContextAccessor contextAccessor)
			: base (contextAccessor)
		{ }

		public override (string Method, string Uri) GenerateEndpoint(PolicyInMemoryRepository.TemplatePolicy policy, object result)
		{
			// TODO : Improve link generation
			string t = GetFormatedPath(policy.Template);
			string r = GetFormatedPath(result.ToString());

			var request = ContextAccessor.HttpContext.Request;
			var host = GetFormatedPath($"{request.Scheme}://{request.Host}");
			var path = GetFormatedPath($"{request.Path}");
			var finalPath = GetFormatedPath($"{t}/{r}");

			return (policy.Method, $"{host}/{finalPath}");
		}
	}
}
