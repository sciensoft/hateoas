using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Sciensoft.Hateoas.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Sciensoft.Hateoas.Providers
{
	internal sealed class HateoasCustomUriProvider : HateoasUriProvider<InMemoryPolicyRepository.CustomPolicy>
	{
		public HateoasCustomUriProvider(IHttpContextAccessor contextAccessor, LinkGenerator linkGenerator)
			: base(contextAccessor, linkGenerator)
		{ }

		public override (string Method, string Uri) GenerateEndpoint(InMemoryPolicyRepository.CustomPolicy policy, object result)
		{
			var request = HttpContext.Request;
			var routeData = HttpContext.GetRouteData();

			string rawResult = result.ToString();
			string formatedResult = GetFormatedPath(ExtractTokenFromUrl(rawResult, routeData.Values));

			if (routeData.Values.Any(r => r.Value.Equals(formatedResult)))
			{
				string virtualPath = LinkGenerator.GetPathByRouteValues(HttpContext, null, routeData.Values);
				string finalVirtualPath = GetFormatedPath(virtualPath);
				return (policy.Method, $"{Host}/{finalVirtualPath}");
			}

			string path = GetFormatedPath(request.Path);
			string finalPath = GetFormatedPath($"{(rawResult.StartsWith("/") ? formatedResult : $"{path}/{formatedResult}")}");

			return (policy.Method, $"{Host}/{finalPath}");
		}

		private string ExtractTokenFromUrl(string path, IDictionary<string, object> routeValues)
		{
			var finalPath = new StringBuilder();
			var regex = new Regex(@"\[(?<token>\w*)\]");

			foreach (string part in path.Split("/", StringSplitOptions.RemoveEmptyEntries))
			{
				string replace = part;
				var match = regex.Match(part);
				if (match.Success)
				{
					var token = match.Groups["token"].Value;
					replace = regex.Replace(part, routeValues[token].ToString());
				}

				finalPath = finalPath.Append($"/{replace}");
			}

			return finalPath.ToString();
		}
	}
}
