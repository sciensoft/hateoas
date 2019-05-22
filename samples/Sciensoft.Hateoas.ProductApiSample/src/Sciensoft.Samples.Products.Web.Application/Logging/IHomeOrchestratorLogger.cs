using System;

namespace Sciensoft.Samples.Products.Web.Application.Logging
{
    public interface IHomeOrchestratorLogger
    {
        void RetrievingContentForAboutPage(string contentPath);

        void FailedToRetrieveContentForAboutPage(string contentPath, Exception ex);
    }
}