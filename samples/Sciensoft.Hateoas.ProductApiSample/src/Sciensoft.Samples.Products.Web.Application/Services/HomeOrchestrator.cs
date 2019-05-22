using Sciensoft.Samples.Products.Web.Application.Logging;
using Sciensoft.Samples.Products.Api.Infrastructure.Repository;
using System;
using System.IO;

namespace Sciensoft.Samples.Products.Web.Application.Services
{
    public class HomeOrchestrator : IHomeOrchestrator
    {
        readonly IHomeOrchestratorLogger _logger;
        readonly FileSystemReadRepository<string> _repository;

        public HomeOrchestrator(
            IHomeOrchestratorLogger logger,
            FileSystemReadRepository<string> repository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public string RetrieveAboutContent()
        {
            string path = $@"{Directory.GetCurrentDirectory()}/README.md";

            try
            {
                _logger.RetrievingContentForAboutPage(path);
                return _repository.Retrieve(path);
            }
            catch (Exception ex)
            {
                _logger.FailedToRetrieveContentForAboutPage(path, ex);
                throw;
            }
        }
    }

    public interface IHomeOrchestrator
    {
        string RetrieveAboutContent();
    }
}
