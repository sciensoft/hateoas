using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Sciensoft.Hateoas.Repositories;
using System;
using System.Linq;

namespace Sciensoft.Hateoas.Providers
{
	internal sealed class HateoasSelfUriProvider : HateoasUriProvider<InMemoryPolicyRepository.SelfPolicy>
	{
		private readonly IActionDescriptorCollectionProvider _actionsProvider;

		public HateoasSelfUriProvider(
			IHttpContextAccessor contextAccessor,
			LinkGenerator linkGenerator,
			IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
			: base(contextAccessor, linkGenerator)
			=> _actionsProvider = actionDescriptorCollectionProvider ?? throw new ArgumentNullException(nameof(actionDescriptorCollectionProvider));

		public override (string Method, string Uri) GenerateEndpoint(InMemoryPolicyRepository.SelfPolicy policy, object result)
		{
			var request = HttpContext.Request;
			var routeData = HttpContext.GetRouteData();

			var localRouteValues = routeData.Values.ToDictionary(k => k.Key, v => v.Value);

			var controllerName = localRouteValues.FirstOrDefault(rv => rv.Key.Equals("controller")).Value.ToString();
			if (!string.IsNullOrWhiteSpace(controllerName))
			{
				var controllerDescriptor = _actionsProvider.ActionDescriptors.Items
					.Where(r => r.RouteValues.Any(rv => rv.Value.Equals(controllerName)));

				var selfActionDescriptor = controllerDescriptor
					.Where(c => c is ControllerActionDescriptor)
					.Where(c => c.EndpointMetadata.Any(em => em is HttpGetAttribute))
					.FirstOrDefault(c => c.Parameters.Any(p => p.ParameterType.IsInstanceOfType(result)));

				if (selfActionDescriptor != null)
				{
					var parameter = selfActionDescriptor.Parameters.FirstOrDefault(p => p.ParameterType.IsInstanceOfType(result));
					localRouteValues = selfActionDescriptor.RouteValues.ToDictionary(r => r.Key, r => (object)r.Value);

					localRouteValues.TryAdd(parameter.Name, result.ToString());
				}
			}

			string rawResult = result.ToString();
			string formatedResult = GetFormatedPath(rawResult);

			if (localRouteValues.Any(r => r.Value.Equals(formatedResult)))
			{
				string virtualPath = LinkGenerator.GetPathByRouteValues(HttpContext, null, localRouteValues);
				string finalVirtualPath = GetFormatedPath(virtualPath);
				return (policy.Method, $"{Host}/{finalVirtualPath}");
			}

			string t = GetFormatedPath(policy.Template);
			string r = GetFormatedPath(result.ToString());

			var path = GetFormatedPath($"{request.Path}");
			var finalPath = GetFormatedPath($"{t}/{r}");

			return (policy.Method, $"{Host}/{path}/{finalPath}");
		}
	}
}
