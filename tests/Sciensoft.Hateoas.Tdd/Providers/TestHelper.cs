using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sciensoft.Hateoas.Tdd.Providers
{
	public static class TestHelper
	{
		public static (
			IHttpContextAccessor ContextAccessor,
			LinkGenerator LinkGenerator,
			IActionDescriptorCollectionProvider ActionDescriptor)
			GetHttpContextHelpers(string path, IDictionary<string, object> routes = null)
		{
			/// HttpRequest
			var httpRequest = new Mock<HttpRequest>();
			httpRequest
				.SetupProperty(x => x.Path, new PathString(path));

			/// HttpContext
			var httpContext = new Mock<HttpContext>();
			httpContext
				.SetupGet(x => x.Request)
				.Returns(httpRequest.Object);

			/// IRoutingFeature
			if (routes == null)
			{
				routes = new Dictionary<string, object>();
			}

			var routeData = new RouteData(new RouteValueDictionary(routes));
			var httpContextFeatureGet = new Mock<IRoutingFeature>();
			httpContextFeatureGet
				.SetupProperty(x => x.RouteData, routeData);

			var httpContextFeature = new FeatureCollection();

			httpContextFeature[typeof(IRoutingFeature)] = httpContextFeatureGet.Object;

			httpContext
				.Setup(x => x.Features)
				.Returns(httpContextFeature);

			/// IHttpContextAccessor
			var contextAccessor = new Mock<IHttpContextAccessor>();
			contextAccessor.SetupProperty(x => x.HttpContext, httpContext.Object);

			/// LinkGenerator
			var linkGenerator = new Mock<LinkGenerator>();

			// IActionDescriptorCollectionProvider
			var actionDescriptor = new Mock<IActionDescriptorCollectionProvider>();
			actionDescriptor
				.SetupGet(a => a.ActionDescriptors)
				.Returns(new ActionDescriptorCollection(new Collection<ActionDescriptor>(new[] {
					new ActionDescriptor()
					{
						RouteValues = routes.ToDictionary(k => k.Key, v => v.Value.ToString())
					}
				}), 0));

			return (contextAccessor.Object, linkGenerator.Object, actionDescriptor.Object);
		}
	}
}
