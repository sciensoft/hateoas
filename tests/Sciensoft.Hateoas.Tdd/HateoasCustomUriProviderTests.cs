using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Moq;
using Sciensoft.Hateoas.Providers;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace Sciensoft.Hateoas.Tdd
{
	public class HateoasCustomUriProviderTests
	{
		[Fact]
		public void ExtractTokenFromUrl_Should_ReplaceTokensFromUrl()
		{
			// Arrange
			var hateoasProvider = new HateoasCustomUriProvider(new Mock<IHttpContextAccessor>().Object, new Mock<LinkGenerator>().Object);

			string path = "[controller]/[action]/hello";
			var rotueValues = new Dictionary<string, object>
			{
				{ "controller", "invoices" },
				{ "action", "get" }
			};

			// Act
			string finalPath = hateoasProvider.GetType()
				.GetMethod("ExtractTokenFromUrl", BindingFlags.Instance | BindingFlags.NonPublic)
				.Invoke(hateoasProvider, new object[] { path, rotueValues })
				.ToString();

			// Assert
			finalPath.Should().NotContain("[controller]");
			finalPath.Should().NotContain("[action]");
		}
	}
}
