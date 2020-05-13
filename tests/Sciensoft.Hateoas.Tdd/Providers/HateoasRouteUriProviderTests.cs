using FluentAssertions;
using Sciensoft.Hateoas.Providers;
using Sciensoft.Hateoas.Repositories;
using Sciensoft.Hateoas.WebSample.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;

namespace Sciensoft.Hateoas.Tdd.Providers
{
	public class HateoasRouteUriProviderTests
	{
		[Fact]
		public void HateoasSelfUriProvider_Should_ReturnDefaultValue_IfControllerIsNotPresent()
		{
			// Arrange
			Guid itemId = Guid.NewGuid();
			Expression<Func<BookViewModel, object>> expression = x => x.Id;
			var selfPolicy = new InMemoryPolicyRepository.RoutePolicy(typeof(BookViewModel), expression, "GetById");

			var helpers = TestHelper.GetHttpContextHelpers(
				"/api/book",
				new Dictionary<string, object>
				{
					{ "action", "get" }
				});

			var uriProvider = new HateoasRouteUriProvider(helpers.ContextAccessor, helpers.LinkGenerator, helpers.ActionDescriptor);

			// Act
			var results = uriProvider.GenerateEndpoint(selfPolicy, itemId);

			// Assert
			results.Should().Be(default);
		}

		[Fact]
		public void HateoasSelfUriProvider_Should_ThrowException_IfNoActionDescriptorProvided()
		{
			// Arrange
			var helpers = TestHelper.GetHttpContextHelpers(
				"/api/book",
				new Dictionary<string, object>
				{
					{ "controller", "book" },
					{ "action", "get" }
				});

			// Act
			Func<HateoasRouteUriProvider> act = () => new HateoasRouteUriProvider(helpers.ContextAccessor, helpers.LinkGenerator, null);

			// Assert
			act.Should().ThrowExactly<ArgumentNullException>();
		}
	}
}