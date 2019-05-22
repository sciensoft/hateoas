using Sciensoft.Samples.Products.AspNetCore.Logging;
using Sciensoft.Samples.Products.AspNetCore.Providers;
using System;

namespace Sciensoft.Samples.Products.Web.Application.Logging
{
    public class HomeOrchestratorLogger : IHomeOrchestratorLogger
    {
        readonly ICoreLogger _logger;
        readonly CorrelationProvider _correlationProvider;
        
        public HomeOrchestratorLogger(
            ICoreLogger logger,
            CorrelationProvider correlationProvider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _correlationProvider = correlationProvider ?? throw new ArgumentNullException(nameof(correlationProvider));
        }

        public void RetrievingContentForAboutPage(string contentPath)
            => _logger.LogInformationAsync(_correlationProvider, $"Retrieving markdown content '{contentPath}' for about page.");

        public void FailedToRetrieveContentForAboutPage(string contentPath, Exception ex)
            => _logger.LogErrorAsync(_correlationProvider, $"Failed to retrieve markdown content '{contentPath}' for about page.", ex);
    }
}
