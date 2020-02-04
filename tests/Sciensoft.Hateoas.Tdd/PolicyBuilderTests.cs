using System;
using System.Linq;
using FluentAssertions;
using Sciensoft.Hateoas.Constants;
using Sciensoft.Hateoas.Exceptions;
using Sciensoft.Hateoas.Repository;
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
			PolicyInMemoryRepository.LinksPolicyInMemory
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
			PolicyInMemoryRepository.LinksPolicyInMemory
				.Any(p => (p as PolicyInMemoryRepository.RoutePolicy) != null
					&& (p as PolicyInMemoryRepository.RoutePolicy).RouteName.Equals(routeName))
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
			string template = "/api/custom";
			var policyBuilder = new PolicyBuilder<object>();

			// Act
			policyBuilder.AddCustomPath(o => o, linkKey, template);

			// Assert
			PolicyInMemoryRepository.LinksPolicyInMemory
				.Any(p => (p as PolicyInMemoryRepository.TemplatePolicy) != null
					&& (p as PolicyInMemoryRepository.TemplatePolicy).Template.Equals(template))
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
			act.Should().Throw<InvalidPolicyConfigurationException>();
		}

		#endregion
	}
}
