using FluentAssertions;
using System.Text.RegularExpressions;
using Xunit;

namespace Sciensoft.Samples.Products.AspNetCore.Tdd
{
    public class ContentNegotiationTests
    {
        [Theory]
        [InlineData("application/json;domain=int", "int")]
        [InlineData("application/json;domain=ProductViewModel", "ProductViewModel")]
        [InlineData("application/json;domain=ProductViewModel.V2", "ProductViewModel.V2")]
        public void ContentNegotiationIdentifier_Should_BeExtractedFromContentType(string contentType, string expectedResult)
        {
            // Arrange
            var regex = new Regex(@";domain=(?<type>\w[.\w+]*)+");

            // Act
            var result = regex.Match(contentType);

            // Assert
            result.Success.Should().BeTrue();
            result.Groups["type"].Value.Should().Be(expectedResult);
        }
    }
}
