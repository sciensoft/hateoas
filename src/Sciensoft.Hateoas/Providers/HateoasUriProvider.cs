using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Sciensoft.Hateoas.Repository;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Sciensoft.Hateoas.Providers
{
	internal class HateoasUriProvider : IHateoasUriProvider
	{
		readonly IHttpContextAccessor _contextAccessor;
		readonly LinkGenerator _linkGenerator;
		readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;

		public HateoasUriProvider(
			IHttpContextAccessor contextAccessor,
			LinkGenerator linkGenerator,
			IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
		{
			_contextAccessor = contextAccessor ?? throw new ArgumentNullException(nameof(contextAccessor));
			_linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
			_actionDescriptorCollectionProvider = actionDescriptorCollectionProvider ?? throw new ArgumentNullException(nameof(actionDescriptorCollectionProvider));
		}

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

			var routeHandler = context.GetRouteData().Routers.OfType<RouteCollection>().First();
			var routeValues = context.GetRouteData().Values;

			var controllerValue = routeValues.FirstOrDefault(rv => rv.Key.Equals("controller")).Value.ToString();
			
			if (string.IsNullOrWhiteSpace(controllerValue))
			{
				return null;
			}

			var routeInfo = _actionDescriptorCollectionProvider.ActionDescriptors.Items
				.Where(r => r.RouteValues.Any(rv => rv.Value.Equals(controllerValue)))
				.Where(r => r.AttributeRouteInfo.Name != null && r.AttributeRouteInfo.Name.Equals(policy.RouteName))
				.FirstOrDefault();
			
			var mexp = (((policy.Expression as LambdaExpression).Body as UnaryExpression).Operand as MemberExpression).Member;
			routeInfo.RouteValues.Add(mexp.Name, result.ToString());

			var routeValueDictionary = new RouteValueDictionary(routeInfo.RouteValues);
			var virtualPathContext = new VirtualPathContext(context, null, routeValueDictionary, policy.RouteName);
			var virtualPath = routeHandler.GetVirtualPath(virtualPathContext).VirtualPath;

			// Routers -> RouteCollection, AttributeRoute, MvcAttributeRouteHandler
			//var mvcAttributeRouteHandler = context.GetRouteData().Routers.First(r => r.GetType().Name.Equals("MvcAttributeRouteHandler"));

			var request = _contextAccessor.HttpContext.Request;
			var host = GetFormatedPath($"{request.Scheme}://{request.Host}");
			var path = GetFormatedPath($"{request.Path}");
			var finalPath = GetFormatedPath($"{virtualPath}");

			return $"{host}/{finalPath}";
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
