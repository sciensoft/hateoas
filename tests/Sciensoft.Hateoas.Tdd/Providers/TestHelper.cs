using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;

namespace Sciensoft.Hateoas.Tdd.Providers
{
	public static class TestHelper
	{
		public static (IHttpContextAccessor ContextAccessor, LinkGenerator LinkGenerator) GetHttpContextHelpers(string path, IDictionary<string, object> routes = null)
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

			return (contextAccessor.Object, linkGenerator.Object);
		}
	}
}
