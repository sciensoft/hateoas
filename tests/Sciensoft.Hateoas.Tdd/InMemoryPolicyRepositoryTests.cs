using FluentAssertions;
using Sciensoft.Hateoas.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;
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
			policy.AddExternalUri(x => x.Title, "https://google.com", "Google Link");

			// Assert
			InMemoryPolicyRepository.InMemoryPolicies
				.First(p => p is InMemoryPolicyRepository.ExternalPolicy)
				.Should()
				.NotBeNull();
		}
		
		[Fact]
		public void InMemoryPolicyRepository_SelfPolicy_Should_SetNameAsTjeCallerMemberName()
		{
			// Arrange
			Expression<Func<object, string>> expression = o => o.ToString();

			// Act
			var policy = new InMemoryPolicyRepository.SelfPolicy(typeof(object), expression);

			// Assert
			policy.Name.Should().Be(nameof(InMemoryPolicyRepository_SelfPolicy_Should_SetNameAsTjeCallerMemberName));
		}

		[Fact]
		public void InMemoryPolicyRepository_SelfPolicy_Should_ThrowArgumentNullException_IfTypeIsNotProvided()
		{
			// Arrange
			// Act
			Action act = () => new InMemoryPolicyRepository.SelfPolicy(null, null);

			// Assert
			act.Should()
				.ThrowExactly<ArgumentNullException>();
		}

		[Fact]
		public void InMemoryPolicyRepository_RoutePolicy_Should_ThrowArgumentNullException_IfRouteNameIsNotProvided()
		{
			// Arrange
			Expression<Func<object, string>> expression = o => o.ToString();

			// Act
			Action act = () => new InMemoryPolicyRepository.RoutePolicy(typeof(object), expression, string.Empty);

			// Assert
			act.Should()
				.ThrowExactly<ArgumentNullException>();
		}
		
		[Fact]
		public void InMemoryPolicyRepository_ExternalPolicy_Should_ThrowArgumentNullException_IfHostsIsNotProvided()
		{
			// Arrange
			Expression<Func<object, string>> expression = o => o.ToString();

			// Act
			Action act = () => new InMemoryPolicyRepository.ExternalPolicy(typeof(object), expression, string.Empty);

			// Assert
			act.Should()
				.ThrowExactly<ArgumentNullException>();
		}

		public class InMemoryTestViewModel
		{
			public string Title { get; set; }
		}
	}
}
