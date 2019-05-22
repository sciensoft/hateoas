using FluentAssertions;
using Moq;
using Sciensoft.Samples.Products.Api.Application.Logging;
using System;
using Xunit;

namespace Sciensoft.Samples.Products.Application.Tdd
{
    public class ProductOrchestratorLoggerTests
    {
        readonly Mock<IProductOrchestratorLogger> _logger;

        public ProductOrchestratorLoggerTests()
            => _logger = new Mock<IProductOrchestratorLogger>();

        #region Retrieve

        [Fact]
        public void ProductOrchestratorLogger_Should_AttemptToRetrieveAllProducts()
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.AttemptToRetrieveAllProducts())
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.AttemptToRetrieveAllProducts();

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Fact]
        public void ProductOrchestratorLogger_Should_FailedToRetrieveAllProducts()
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.FailedToRetrieveAllProducts(It.IsAny<Exception>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.FailedToRetrieveAllProducts(new InvalidOperationException());

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_AttemptToRetrieveProduct(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.AttemptToRetrieveProduct(It.IsAny<string>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.AttemptToRetrieveProduct(message);

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_FailedToRetrieveProduct(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.FailedToRetrieveProduct(It.IsAny<string>(), It.IsAny<Exception>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.FailedToRetrieveProduct(message, new InvalidOperationException(message));

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_AttemptToRetrieveProductVersion(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.AttemptToRetrieveProductVersion(It.IsAny<string>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.AttemptToRetrieveProductVersion(message);

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_FailedToRetrieveProductVersion(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.FailedToRetrieveProductVersion(It.IsAny<string>(), It.IsAny<Exception>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.FailedToRetrieveProductVersion(message, new InvalidOperationException(message));

            // Assert
            wasCalled.Should().BeTrue();
        }

        #endregion

        #region Create

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_AttemptToCreateProduct(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.AttemptToCreateProduct(It.IsAny<string>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.AttemptToCreateProduct(message);

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_SuccessfullyCreatedProduct(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.SuccessfullyCreatedProduct(It.IsAny<string>(), It.IsAny<object>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.SuccessfullyCreatedProduct(message, new { message });

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_FailedToCreateProduct(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.FailedToCreateProduct(It.IsAny<string>(), It.IsAny<Exception>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.FailedToCreateProduct(message, new InvalidOperationException(message));

            // Assert
            wasCalled.Should().BeTrue();
        }

        #endregion

        #region Update

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_AttemptToPatchUpdateProduct(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.AttemptToPatchUpdateProduct(It.IsAny<string>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.AttemptToPatchUpdateProduct(message);

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_SuccessfullyPatchUpdatedProduct(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.SuccessfullyPatchUpdatedProduct(It.IsAny<string>(), It.IsAny<object>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.SuccessfullyPatchUpdatedProduct(message, new { message });

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_FailedToPatchUpdateProduct(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.FailedToPatchUpdateProduct(It.IsAny<string>(), It.IsAny<Exception>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.FailedToPatchUpdateProduct(message, new InvalidOperationException(message));

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_AttemptToUpdateProduct(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.AttemptToUpdateProduct(It.IsAny<string>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.AttemptToUpdateProduct(message);

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_SuccessfullyUpdatedProduct(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.SuccessfullyUpdatedProduct(It.IsAny<string>(), It.IsAny<object>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.SuccessfullyUpdatedProduct(message, new { message });

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_FailedToUpdateProduct(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.FailedToUpdateProduct(It.IsAny<string>(), It.IsAny<Exception>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.FailedToUpdateProduct(message, new InvalidOperationException(message));

            // Assert
            wasCalled.Should().BeTrue();
        }

        #endregion

        #region Delete

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_AttemptToDeleteProduct(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.AttemptToDeleteProduct(It.IsAny<string>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.AttemptToDeleteProduct(message);

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_SuccessfullyDeletedProduct(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.SuccessfullyDeletedProduct(It.IsAny<string>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.SuccessfullyDeletedProduct(message);

            // Assert
            wasCalled.Should().BeTrue();
        }

        [Theory]
        [InlineData("Hello Test")]
        public void ProductOrchestratorLogger_Should_FailedToDeleteProduct(string message)
        {
            // Arrange
            bool wasCalled = false;

            _logger.Setup(s => s.FailedToDeleteProduct(It.IsAny<string>(), It.IsAny<Exception>()))
                .Callback(() => wasCalled = true);

            // Act
            _logger.Object.FailedToDeleteProduct(message, new InvalidOperationException(message));

            // Assert
            wasCalled.Should().BeTrue();
        }

        #endregion
    }
}
