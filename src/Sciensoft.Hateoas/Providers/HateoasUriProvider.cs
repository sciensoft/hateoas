using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Sciensoft.Hateoas.Repository;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sciensoft.Hateoas.Providers
{
	internal class HateoasUriProvider : IHateoasUriProvider
	{
		readonly IHttpContextAccessor _contextAccessor;

		public HateoasUriProvider(IHttpContextAccessor contextAccessor)
			=> _contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));

		public string GenerateUri(PolicyInMemoryRepository.Policy policy, object result)
		{
			AssureIsNotNull(policy, nameof(policy));
			AssureIsNotNull(result, nameof(result));

			switch (policy)
			{
				case PolicyInMemoryRepository.TemplatePolicy genericPolicy:
					return GenerateUri(genericPolicy, result);
				case PolicyInMemoryRepository.RoutePolicy routePolicy:
					return GenerateUri(routePolicy, result);
				default:
					return null;
			}
		}

		private string GenerateUri(PolicyInMemoryRepository.TemplatePolicy policy, object result)
		{
			string t = GetFormatedPath(policy.Template);
			string r = GetFormatedPath(result.ToString());

			var request = _contextAccessor.HttpContext.Request;
			var host = GetFormatedPath($"{request.Scheme}://{request.Host}");
			var path = GetFormatedPath($"{request.Path}");
			var finalPath = GetFormatedPath($"{t}/{r}");

			return $"{host}/{path}/{finalPath}";
		}

		private string GenerateUri(PolicyInMemoryRepository.RoutePolicy policy, object result)
		{
			var context = _contextAccessor.HttpContext;

			// Other types -> RouteCollection, AttributeRoute
			var mvcAttributeRouteHandler = context.GetRouteData().Routers.OfType<MvcAttributeRouteHandler>().First();

			var mvcActions = mvcAttributeRouteHandler.Actions;
			var routeInfo = mvcActions.FirstOrDefault(a => a.AttributeRouteInfo.Name != null && a.AttributeRouteInfo.Name.Equals(policy.RouteName)).AttributeRouteInfo;

			var request = _contextAccessor.HttpContext.Request;
			var host = GetFormatedPath($"{request.Scheme}://{request.Host}");
			var path = GetFormatedPath($"{request.Path}");
			var finalPath = GetFormatedPath($"{routeInfo.Template}/{result}");

			return $"{host}/{path}/{finalPath}";
		}

		private string GetFormatedPath(string path)
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

		private void AssureIsNotNull(object @object, string name)
		{
			if (@object == null)
			{
				throw new ArgumentNullException(nameof(name));
			}
		}
	}

	internal interface IHateoasUriProvider
	{
		string GenerateUri(PolicyInMemoryRepository.Policy policy, object result);
	}
}
