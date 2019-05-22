using Sciensoft.Samples.Products.AspNetCore.Providers;
using System;

namespace Sciensoft.Samples.Products.AspNetCore.Logging
{
    internal class ExceptionHandlerLogger : IExceptionHandlerLogger
    {
        readonly ICoreLogger _logger;
        readonly CorrelationProvider _correlationProvider;

        public ExceptionHandlerLogger(ICoreLogger logger, CorrelationProvider correlationProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _correlationProvider = correlationProvider ?? throw new ArgumentNullException(nameof(correlationProvider));
        }

        public void StartingProcessingRequest(params object[] data)
            => _logger.LogInformationAsync(_correlationProvider, "Attempting to process request.", data: data);

        public void SuccessfullyProcessedRequest(params object[] data)
            => _logger.LogInformationAsync(_correlationProvider, "Successfjully processed request.", data: data);

        public void FailedProcessingRequest(Exception ex, string message, params object[] data)
            => _logger.LogWarningAsync(_correlationProvider, $"Failed to process request with reason '{message}'.", ex, data: data);
    }
}
