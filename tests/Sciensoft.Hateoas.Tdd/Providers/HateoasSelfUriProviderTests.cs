using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Sciensoft.Hateoas.Providers;
using Sciensoft.Hateoas.Repositories;
using Sciensoft.Hateoas.WebSample.Models;
using System;
using System.Linq.Expressions;
using Xunit;

namespace Sciensoft.Hateoas.Tdd.Providers
{
	public class HateoasSelfUriProviderTests
	{
		[Fact]
		public void HateoasSelfUriProvider_Should_GenerateEndpoint_BasedOnRequest()
		{
			// Arrange
			Guid itemId = Guid.NewGuid();
			Expression<Func<BookViewModel, object>> expression = x => x.Id;
			var selfPolicy = new InMemoryPolicyRepository.SelfPolicy(typeof(BookViewModel), expression, "GetById")
			{
				Method = HttpMethods.Get
			};

			var helpers = TestHelper.GetHttpContextHelpers("/api/book");
			var uriProvider = new HateoasSelfUriProvider(helpers.ContextAccessor, helpers.LinkGenerator);

			// Act
			var results = uriProvider.GenerateEndpoint(selfPolicy, itemId);

			// Assert
			results.Should().NotBeNull();
			results.Method.Should().Be(HttpMethods.Get);
			results.Uri.Should().EndWith($"/api/book/{itemId}");
		}
	}
}