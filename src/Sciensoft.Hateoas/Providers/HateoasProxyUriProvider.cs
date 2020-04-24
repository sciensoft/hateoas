using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Sciensoft.Hateoas.Abstractions;
using Sciensoft.Hateoas.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Sciensoft.Hateoas.Providers
{
	internal class HateoasProxyUriProvider : HateoasUriProvider<PolicyInMemoryRepository.Policy>
	{
		private readonly LinkGenerator _linkGenerator;
		private readonly IActionDescriptorCollectionProvider _actionsProvider;

		public HateoasProxyUriProvider(
			LinkGenerator linkGenerator,
			IHttpContextAccessor contextAccessor,
			IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
			: base (contextAccessor)
		{
			_linkGenerator = linkGenerator ?? throw new ArgumentNullException(nameof(linkGenerator));
			_actionsProvider = actionDescriptorCollectionProvider ?? throw new ArgumentNullException(nameof(actionDescriptorCollectionProvider));
		}

		public override (string Method, string Uri) GenerateEndpoint(PolicyInMemoryRepository.Policy policy, object result)
		{
			AssureIsNotNull(policy, nameof(policy));
			AssureIsNotNull(result, nameof(result));

			switch (policy)
			{
				case PolicyInMemoryRepository.TemplatePolicy genericPolicy:
					return GenerateEndpoint(genericPolicy, result);
				case PolicyInMemoryRepository.RoutePolicy routePolicy:
					return GenerateEndpoint(routePolicy, result);
				default:
					return default;
			}
		}

		private (string Method, string Uri) GenerateEndpoint(PolicyInMemoryRepository.TemplatePolicy policy, object result)
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

		private (string Method, string Uri) GenerateEndpoint(PolicyInMemoryRepository.RoutePolicy policy, object result)
		{
			// TODO : Code Refactor this method to move some non-concerned logic away
			var context = ContextAccessor.HttpContext;
			var routeData = context.GetRouteData();

			// Routers -> RouteCollection, AttributeRoute, MvcAttributeRouteHandler
			//var mvcAttributeRouteHandler = context.GetRouteData().Routers.First(r => r.GetType().Name.Equals("MvcAttributeRouteHandler"));

			var controllerValue = routeData.Values.FirstOrDefault(rv => rv.Key.Equals("controller")).Value.ToString();
			if (string.IsNullOrWhiteSpace(controllerValue))
			{
				return default;
			}

			var routeInfo = _actionsProvider.ActionDescriptors.Items
				.FirstOrDefault(r =>
					r.RouteValues.Any(rv => rv.Value.Equals(controllerValue)) &&
					r.AttributeRouteInfo.Name != null && r.AttributeRouteInfo.Name.Equals(policy.RouteName));

			// TODO : Add better NULL object validation
			var expressionMember = (((policy.Expression as LambdaExpression).Body as UnaryExpression).Operand as MemberExpression).Member;
			routeInfo.RouteValues.TryAdd(expressionMember.Name, result.ToString());

			var httpMethodMetadata = routeInfo.EndpointMetadata.FirstOrDefault(x => x is HttpMethodMetadata) as HttpMethodMetadata;
			var httpMethod = httpMethodMetadata.HttpMethods.FirstOrDefault();

			var virtualPath = _linkGenerator.GetPathByRouteValues(context, policy.RouteName, routeInfo.RouteValues);

			var request = context.Request;
			var host = GetFormatedPath($"{request.Scheme}://{request.Host}");
			var finalPath = GetFormatedPath($"{virtualPath}");

			return (httpMethod, $"{host}/{finalPath}");
		}
	}
}
