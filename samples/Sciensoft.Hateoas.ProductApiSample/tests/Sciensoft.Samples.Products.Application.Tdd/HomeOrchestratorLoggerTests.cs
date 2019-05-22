using FluentAssertions;
using Moq;
using Sciensoft.Samples.Products.Web.Application.Logging;
using System;
using Xunit;

namespace Sciensoft.Samples.Products.Application.Tdd
{
    public class HomeOrchestratorLoggerTests
    {
        readonly Mock<IHomeOrchestratorLogger> _logger;

        public HomeOrchestratorLoggerTests()
            => _logger = new Mock<IHomeOrchestratorLogger>();

        [Theory]
        [InlineData("Hello-Test")]
        public void ProductOrchestratorLogger_Should_RetrievingContentForAboutPage(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.RetrievingContentForAboutPage(It.IsAny<string>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.RetrievingContentForAboutPage(message);

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("Hello-Test")]
        public void ProductOrchestratorLogger_Should_FailedToRetrieveContentForAboutPage(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.FailedToRetrieveContentForAboutPage(It.IsAny<string>(), It.IsAny<Exception>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.FailedToRetrieveContentForAboutPage(message, new InvalidOperationException(message));

            // Assert
            wasCalled.Should().BeTrue();
        }
    }
}
