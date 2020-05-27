using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Moq;
using Newtonsoft.Json.Linq;
using Sciensoft.Hateoas.Constants;
using Sciensoft.Hateoas.Providers;
using Sciensoft.Hateoas.Repositories;
using Sciensoft.Hateoas.WebSample;
using Sciensoft.Hateoas.WebSample.Controllers;
using Sciensoft.Hateoas.WebSample.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sciensoft.Hateoas.Tdd.Providers
{
	public class HateoasResultProviderTests
	{
		private readonly TestServer _server;
		private readonly HttpClient _client;

		public HateoasResultProviderTests()
		{
			var host = new HostBuilder()
				.ConfigureWebHost(hostBuilder =>
				{
					hostBuilder
						.UseTestServer()
						.UseStartup<Startup>();
				})
				.Build();

			host.StartAsync();

			_server = host.GetTestServer();
			_client = _server.CreateClient();
		}

		public class LinkViewModel
		{
			public string Method { get; set; }
			public string Uri { get; set; }
			public string Relation { get; set; }
			public string Message { get; set; }
		}

		[Fact]
		public async Task HateoasRouteUriProvider_Should_GenerateLinks_BasedOnRequestEndpoint()
		{
			// Arrange
			string bookId = "83389205-B1C9-4523-A3BB-85D7255546F9";

			// Act
			var request = await _client.GetAsync($"api/books/{bookId}");
			var jsonPayload = JObject.Parse(await request.Content.ReadAsStringAsync());
			var links = jsonPayload?.SelectToken("links")?.Values<dynamic>();

			// Assert
			request.StatusCode.Should().Be(HttpStatusCode.OK);
			links.Should().HaveCountGreaterThan(0);
		}

		[Fact]
		public async Task HateoasRouteUriProvider_Should_GenerateSelfLink_BasedOnRequestEndpoint()
		{
			// Arrange
			string bookId = "83389205-B1C9-4523-A3BB-85D7255546F9";

			// Act
			var request = await _client.GetAsync($"api/books/{bookId}");
			var jsonPayload = JObject.Parse(await request.Content.ReadAsStringAsync());
			var links = jsonPayload?.SelectToken("links")?.Children().Select(jo => jo.ToObject<LinkViewModel>());

			// Assert
			request.StatusCode.Should().Be(HttpStatusCode.OK);
			links.Should().NotBeNull();
			links.Should().HaveCountGreaterThan(0);
			links.Single(l => l.Relation.Equals(PolicyConstants.Self) && l.Method.Equals(HttpMethods.Get)).Should().NotBeNull();
		}

		[Fact]
		public async Task HateoasRouteUriProvider_Should_GenerateRouteLinks_BasedOnRequestEndpoint()
		{
			// Arrange
			string bookId = "83389205-B1C9-4523-A3BB-85D7255546F9";
			var routePolicies = InMemoryPolicyRepository.InMemoryPolicies
				.Where(p => p is InMemoryPolicyRepository.RoutePolicy)
				.Where(p => p.Name.Equals(BookController.UpdateBookById) 
					|| p.Name.Equals(BookController.DeleteBookById))
				.ToList();

			// Act
			var request = await _client.GetAsync($"api/books/{bookId}");
			var jsonPayload = JObject.Parse(await request.Content.ReadAsStringAsync());
			var links = jsonPayload?.SelectToken("links")?.Children()
				.Select(jo => jo.ToObject<LinkViewModel>())
				.Where(m => routePolicies.Any(p => p.Name.Equals(m.Relation)));

			// Assert
			request.StatusCode.Should().Be(HttpStatusCode.OK);
			links.Should().NotBeNull();
			links.Should().HaveCountGreaterThan(1);
		}

		[Fact(Skip = "Not applicable!")]
		public void HateoasRouteUriProvider_Should_GenerateEndpoint_BasedOnRouteNameAndRouteData_OLD()
		{
			// Arrange
			Guid itemId = Guid.NewGuid();
			Expression<Func<BookViewModel, object>> expression = x => x.Id;
			var selfPolicy = new InMemoryPolicyRepository.RoutePolicy(
				typeof(BookViewModel), expression, "GetById");

			var actionDescriptor = new ActionDescriptor
			{
				AttributeRouteInfo = new AttributeRouteInfo { Name = "GetById" },
				EndpointMetadata = new List<object>
				{
					new HttpMethodMetadata(new [] { "Get" })
				}
			};

			actionDescriptor.RouteValues = new Dictionary<string, string>
			{
				{ "action", "Get" },
				{ "controller", "Books" }
			};

			var helpers = TestHelper.GetHttpContextHelpers("/api/book", actionDescriptor.RouteValues.ToDictionary(x => x.Key, x => (object)x.Value));
			helpers.ContextAccessor.HttpContext.Request.RouteValues = new RouteValueDictionary(actionDescriptor.RouteValues);

			var actionDescriptorCollection = new ReadOnlyCollection<ActionDescriptor>(new[] { actionDescriptor });
			var actionDescriptorProvider = new Mock<IActionDescriptorCollectionProvider>();
			actionDescriptorProvider
				.SetupGet(x => x.ActionDescriptors)
				.Returns(new ActionDescriptorCollection(actionDescriptorCollection, 1));

			var uriProvider = new HateoasRouteUriProvider(helpers.ContextAccessor, helpers.LinkGenerator, actionDescriptorProvider.Object);

			// Act
			var results = uriProvider.GenerateEndpoint(selfPolicy, itemId);

			// Assert
			results.Should().NotBeNull();
			results.Method.Should().Be(HttpMethods.Get);
			results.Uri.Should().EndWith($"/api/book/{itemId}");
		}
	}
}