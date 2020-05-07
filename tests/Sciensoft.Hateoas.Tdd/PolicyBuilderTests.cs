using FluentAssertions;
using Sciensoft.Hateoas.Constants;
using Sciensoft.Hateoas.Exceptions;
using Sciensoft.Hateoas.Repositories;
using System;
using System.Linq;
using Xunit;

namespace Sciensoft.Hateoas.Tdd
{
	public class PolicyBuilderTests
	{
		#region AddSelf

		[Fact]
		public void AddSelf_Should_AddLinkToPolicyInMemoryRepository()
		{
			// Arrange
			var policyBuilder = new PolicyBuilder<object>();

			// Act
			policyBuilder.AddSelf(o => o);

			// Assert
			InMemoryPolicyRepository.InMemoryPolicies
				.Any(p => p.Name.Equals(PolicyConstants.Self))
				.Should().BeTrue();
		}

		[Fact]
		public void AddSelf_Should_Throw_ArgumentNullException()
		{
			// Arrange
			var policyBuilder = new PolicyBuilder<object>();

			// Act
			Action act = () => policyBuilder.AddSelf(null);

			// Assert
			act.Should().Throw<ArgumentNullException>();
		}

		#endregion

		#region AddRoute

		[Fact]
		public void AddRoute_Should_AddLinkToPolicyInMemoryRepository()
		{
			// Arrange
			string routeName = "ThisIsMyRoute";
			var policyBuilder = new PolicyBuilder<object>();

			// Act
			policyBuilder.AddRoute(o => o, routeName);

			// Assert
			InMemoryPolicyRepository.InMemoryPolicies
				.Any(p => (p is InMemoryPolicyRepository.RoutePolicy)
					&& (p as InMemoryPolicyRepository.RoutePolicy).RouteName.Equals(routeName))
				.Should().BeTrue();
		}

		[Fact]
		public void AddRoute_Should_Throw_InvalidPolicyConfigurationException()
		{
			// Arrange
			var policyBuilder = new PolicyBuilder<object>();

			// Act
			Action act = () => policyBuilder.AddRoute(o => o, null);

			// Assert
			act.Should().Throw<InvalidPolicyConfigurationException>();
		}

		#endregion

		#region AddCustom

		[Fact]
		public void AddCustom_Should_AddLinkToPolicyInMemoryRepository()
		{
			// Arrange
			string linkKey = "CustomLink";
			var policyBuilder = new PolicyBuilder<object>();

			// Act
			policyBuilder.AddCustomPath(o => o, linkKey);

			// Assert
			InMemoryPolicyRepository.InMemoryPolicies
				.Any(p => p is InMemoryPolicyRepository.CustomPolicy)
				.Should().BeTrue();
		}

		[Fact]
		public void AddCustom_Should_Throw_InvalidPolicyConfigurationException()
		{
			// Arrange
			var policyBuilder = new PolicyBuilder<object>();

			// Act
			Action act = () => policyBuilder.AddCustomPath(o => o, null);

			// Assert
			act.Should().Throw<InvalidPolicyConfigurationException>()
				.WithMessage("Custom Policy requires 'linkKey' argument.");
		}

		#endregion

		#region AddExternal

		[Fact]
		public void AddExternal_Should_AddLinkToPolicyInMemoryRepository()
		{
			// Arrange
			string host = "https://google.com";
			string linkKey = "ExternalLink";
			var policyBuilder = new PolicyBuilder<object>();

			// Act
			policyBuilder.AddExternalUri(o => o, host, linkKey);

			// Assert
			InMemoryPolicyRepository.InMemoryPolicies
				.Any(p => p is InMemoryPolicyRepository.CustomPolicy)
				.Should().BeTrue();
		}

		[Fact]
		public void AddExternal_Should_Throw_InvalidPolicyConfigurationException_IfHostArgumentIsNotProvided()
		{
			// Arrange
			var policyBuilder = new PolicyBuilder<object>();

			// Act
			Action act = () => policyBuilder.AddExternalUri(o => o, string.Empty, null);

			// Assert
			act.Should().Throw<InvalidPolicyConfigurationException>()
				.WithMessage("External Policy requires 'host' argument.");
		}

		[Fact]
		public void AddExternal_Should_Throw_InvalidPolicyConfigurationExceptionIfLinkKeyArgumentIsNotProvided()
		{
			// Arrange
			var policyBuilder = new PolicyBuilder<object>();

			// Act
			Action act = () => policyBuilder.AddExternalUri(o => o, "https://google.com", null);

			// Assert
			act.Should().Throw<InvalidPolicyConfigurationException>()
				.WithMessage("External Policy requires 'linkKey' argument.");
		}

		#endregion
	}
}
