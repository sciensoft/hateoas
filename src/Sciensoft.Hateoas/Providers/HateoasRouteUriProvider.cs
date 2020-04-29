using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Sciensoft.Hateoas.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sciensoft.Hateoas.Providers
{
	internal class HateoasRouteUriProvider : HateoasUriProvider<InMemoryPolicyRepository.RoutePolicy>
	{
		private readonly IActionDescriptorCollectionProvider _actionsProvider;

		public HateoasRouteUriProvider(
			IHttpContextAccessor contextAccessor,
			LinkGenerator linkGenerator,
			IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
			: base(contextAccessor, linkGenerator)
		{
			_actionsProvider = actionDescriptorCollectionProvider ?? throw new ArgumentNullException(nameof(actionDescriptorCollectionProvider));
		}

		public override (string Method, string Uri) GenerateEndpoint(InMemoryPolicyRepository.RoutePolicy policy, object result)
		{
			var routeData = HttpContext.GetRouteData();

			var controllerValue = routeData.Values.FirstOrDefault(rv => rv.Key.Equals("controller")).Value.ToString();
			if (string.IsNullOrWhiteSpace(controllerValue))
			{
				return default;
			}

			var routeInfo = _actionsProvider.ActionDescriptors.Items
				.FirstOrDefault(r =>
					r.RouteValues.Any(rv => rv.Value.Equals(controllerValue)) &&
					r.AttributeRouteInfo.Name != null && r.AttributeRouteInfo.Name.Equals(policy.RouteName));

			var expressionMember = (((policy.Expression as LambdaExpression)?.Body as UnaryExpression)?.Operand as MemberExpression)?.Member;
			routeInfo.RouteValues.TryAdd(expressionMember?.Name, result.ToString());

			var httpMethodMetadata = routeInfo.EndpointMetadata.FirstOrDefault(x => x is HttpMethodMetadata) as HttpMethodMetadata;
			var httpMethod = httpMethodMetadata.HttpMethods.FirstOrDefault();

			string virtualPath = LinkGenerator.GetPathByRouteValues(HttpContext, policy.RouteName, routeInfo.RouteValues);
			string finalPath = GetFormatedPath($"{virtualPath}");

			return (httpMethod, $"{Host}/{finalPath}");
		}
	}
}
