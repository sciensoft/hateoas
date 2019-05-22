using FluentAssertions;
using Sciensoft.Samples.Products.Api.Application.Services;
using Xunit;

namespace Sciensoft.Samples.Products.Application.Tdd
{
    public class EntityTagHashProviderTests
    {
        [Fact]
        public void EntityTagHashProvider_Should_CreateHash()
        {
            // Arrange
            int version = 1;
            var hashProvider = new EntityTagHashProvider();

            // Act
            var hash = hashProvider.CreateHash(version.ToString());

            // Assert
            hash.Should().NotBeNullOrWhiteSpace();
        }

        [Theory]
        [InlineData("1", "1")]
        [InlineData("Hello", "Hello")]
        [InlineData("A", "A")]
        [InlineData("20.5", "20.5")]
        public void EntityTagHashProvider_Should_BeEqualsForSameValue(string p1, string p2)
        {
            // Arrange
            var hashProvider = new EntityTagHashProvider();

            // Act
            var hash = hashProvider.CreateHash(p1);
            var hash2 = hashProvider.CreateHash(p2);

            // Assert
            hash.Should().Be(hash2);
        }

        [Theory]
        [InlineData("1", "2")]
        [InlineData("Hello", "Bye")]
        [InlineData("A", "B")]
        [InlineData("20.5", "41")]
        public void EntityTagHashProvider_Should_NotBeEquals(string p1, string p2)
        {
            // Arrange
            var hashProvider = new EntityTagHashProvider();

            // Act
            var hash = hashProvider.CreateHash(p1);
            var hash2 = hashProvider.CreateHash(p2);

            // Assert
            hash.Should().NotBe(hash2);
        }
    }
}
