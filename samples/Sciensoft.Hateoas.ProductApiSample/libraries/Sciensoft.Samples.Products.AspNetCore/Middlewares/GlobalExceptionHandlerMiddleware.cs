using Microsoft.AspNetCore.Http;
using Sciensoft.Samples.Products.AspNetCore.Abstractions;
using Sciensoft.Samples.Products.AspNetCore.Logging;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Sciensoft.Samples.Products.AspNetCore.Middlewares
{
    internal class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        readonly IExceptionHandler _exceptionHandler;
        readonly IExceptionHandlerLogger _logger;

        public GlobalExceptionHandlerMiddleware(
            IExceptionHandler exceptionHandler,
            IExceptionHandlerLogger logger)
        {
            _exceptionHandler = exceptionHandler;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                _logger.StartingProcessingRequest(new { context.Request.Path, context.Request.PathBase });
                await next?.Invoke(context);
                _logger.SuccessfullyProcessedRequest(new { context.Request.Path, context.Request.PathBase });
            }
            catch (Exception ex)
            {
                _logger.FailedProcessingRequest(ex, ex.Message, new { context.Request.Path });
                if (_exceptionHandler != null)
                    _exceptionHandler.Handle(context, ex);
                else
                    throw;
            }
        }

        public void Handle(Exception ex, HttpContext context)
        {
            AssureArgumentIsNotNull(context, nameof(context));
            AssureArgumentIsNotNull(ex, nameof(ex));

            byte[] responseMessage = Encoding.UTF8.GetBytes($"Something wrong happened. {ex.Message}");
            context.Response.Body = new MemoryStream(responseMessage);

            switch (ex)
            {
                case InvalidOperationException _:
                case ArgumentNullException _:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
        }

        private void AssureArgumentIsNotNull(object argument, string argumentName = "")
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName);
        }
    }
}
