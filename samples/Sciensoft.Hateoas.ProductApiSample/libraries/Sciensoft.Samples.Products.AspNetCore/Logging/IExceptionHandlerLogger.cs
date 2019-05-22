using System;

namespace Sciensoft.Samples.Products.AspNetCore.Logging
{
    internal interface IExceptionHandlerLogger
    {
        void StartingProcessingRequest(params object[] data);

        void SuccessfullyProcessedRequest(params object[] data);

        void FailedProcessingRequest(Exception ex, string message, params object[] data);
    }
}
