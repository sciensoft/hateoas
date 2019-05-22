using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging;
using Moq;
using Sciensoft.Samples.Products.AspNetCore.Logging;
using Sciensoft.Samples.Products.AspNetCore.Providers;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Sciensoft.Samples.Products.AspNetCore.Tdd
{
    public class CoreLoggerTests
    {
        [Fact]
        public async Task CoreLogger_Should_LogVerboseOrTrace()
        {
            // Arrange
            bool wasLogExecuted = false;
            var helpers = GetHelpers();

            helpers.LoggerStub
                .Setup(l => l.Log(LogLevel.Trace, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()))
                .Callback(() => wasLogExecuted = true);

            var coreLogger = new CoreLogger(helpers.LoggerStub.Object);

            // Act
            await coreLogger.LogVerboseAsync(helpers.Correlation, "Testing loggers", helpers.Causation);

            // Assert
            wasLogExecuted.Should().BeTrue();
        }

        [Fact]
        public async Task CoreLogger_Should_LogDebug()
        {
            // Arrange
            bool wasLogExecuted = false;
            var helpers = GetHelpers();

            helpers.LoggerStub
                .Setup(l => l.Log(LogLevel.Debug, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()))
                .Callback(() => wasLogExecuted = true);

            var coreLogger = new CoreLogger(helpers.LoggerStub.Object);

            // Act
            await coreLogger.LogDebugAsync(helpers.Correlation, "Testing loggers", helpers.Causation);

            // Assert
            wasLogExecuted.Should().BeTrue();
        }

        [Fact]
        public async Task CoreLogger_Should_LogInformation()
        {
            // Arrange
            bool wasLogExecuted = false;
            var helpers = GetHelpers();

            helpers.LoggerStub
                .Setup(l => l.Log(LogLevel.Information, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()))
                .Callback(() => wasLogExecuted = true);

            var coreLogger = new CoreLogger(helpers.LoggerStub.Object);

            // Act
            await coreLogger.LogInformationAsync(helpers.Correlation, "Testing loggers", helpers.Causation);

            // Assert
            wasLogExecuted.Should().BeTrue();
        }

        [Fact]
        public async Task CoreLogger_Should_LogWarning()
        {
            // Arrange
            bool wasLogExecuted = false;
            var helpers = GetHelpers();

            helpers.LoggerStub
                .Setup(l => l.Log(LogLevel.Warning, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()))
                .Callback(() => wasLogExecuted = true);

            var coreLogger = new CoreLogger(helpers.LoggerStub.Object);

            // Act
            await coreLogger.LogWarningAsync(helpers.Correlation, "Testing loggers", causationId: helpers.Causation);

            // Assert
            wasLogExecuted.Should().BeTrue();
        }

        [Fact]
        public async Task CoreLogger_Should_LogError()
        {
            // Arrange
            bool wasLogExecuted = false;
            var helpers = GetHelpers();

            helpers.LoggerStub
                .Setup(l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<object>(), It.IsAny<Exception>(), It.IsAny<Func<object, Exception, string>>()))
                .Callback(() => wasLogExecuted = true);

            var coreLogger = new CoreLogger(helpers.LoggerStub.Object);

            // Act
            await coreLogger.LogErrorAsync(helpers.Correlation, "Testing loggers", new InvalidOperationException("Testing loggers"), helpers.Causation);

            // Assert
            wasLogExecuted.Should().BeTrue();
        }

        private (Mock<ILogger<CoreLogger>> LoggerStub, CorrelationProvider Correlation, CausationProvider Causation) GetHelpers()
        {
            var loggerStub = new Mock<ILogger<CoreLogger>>();

            var httpContext = new DefaultHttpContext();
            var httpRequest = new DefaultHttpRequest(httpContext);
            var httpContextAccessor = new HttpContextAccessor
            {
                HttpContext = httpContext
            };

            var correlation = CorrelationProvider.Create();
            var causation = new CausationProvider(httpContextAccessor);
            httpRequest.Headers.Add("X-Correlation-ID", correlation.CorrelationId.ToString());

            return (loggerStub, correlation, causation);
        }
    }
}
