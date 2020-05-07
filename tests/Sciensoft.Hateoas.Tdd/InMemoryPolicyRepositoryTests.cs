using FluentAssertions;
using Sciensoft.Hateoas.Repositories;
using System.Linq;
using Xunit;

namespace Sciensoft.Hateoas.Tdd
{
	public class InMemoryPolicyRepositoryTests
	{
		[Fact]
		public void InMemoryPolicyRepository_Should_AddSelfPolicy_ToInMemoryDictionary()
		{
			// Arrange
			var policy = new PolicyBuilder<InMemoryTestViewModel>();

			// Act
			policy.AddSelf(x => x.Title);

			// Assert
			InMemoryPolicyRepository.InMemoryPolicies
				.First(p => p is InMemoryPolicyRepository.SelfPolicy)
				.Should()
				.NotBeNull();
		}

		[Fact]
		public void InMemoryPolicyRepository_Should_AddRoutePolicy_ToInMemoryDictionary()
		{
			// Arrange
			var policy = new PolicyBuilder<InMemoryTestViewModel>();

			// Act
			policy.AddRoute(x => x.Title, "RouteName");

			// Assert
			InMemoryPolicyRepository.InMemoryPolicies
				.First(p => p is InMemoryPolicyRepository.RoutePolicy)
				.Should()
				.NotBeNull();
		}

		[Fact]
		public void InMemoryPolicyRepository_Should_AddCustomPolicy_ToInMemoryDictionary()
		{
			// Arrange
			var policy = new PolicyBuilder<InMemoryTestViewModel>();

			// Act
			policy.AddCustomPath(x => x.Title, "key");

			// Assert
			InMemoryPolicyRepository.InMemoryPolicies
				.First(p => p is InMemoryPolicyRepository.CustomPolicy)
				.Should()
				.NotBeNull();
		}

		[Fact]
		public void InMemoryPolicyRepository_Should_AddExternalUri_ToInMemoryDictionary()
		{
			// Arrange
			var policy = new PolicyBuilder<InMemoryTestViewModel>();

			// Act
			policy.AddExternalUri(x => x.Title, "https://google.com", "key");

			// Assert
			InMemoryPolicyRepository.InMemoryPolicies
				.First(p => p is InMemoryPolicyRepository.ExternalPolicy)
				.Should()
				.NotBeNull();
		}

		public class InMemoryTestViewModel
		{
			public string Title { get; set; }
		}
	}
}
