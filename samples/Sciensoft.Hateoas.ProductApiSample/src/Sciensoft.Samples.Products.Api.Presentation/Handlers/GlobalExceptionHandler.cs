using Microsoft.AspNetCore.Http;
using Sciensoft.Samples.Products.AspNetCore.Abstractions;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Sciensoft.Samples.Products.Api.Presentation.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public void Handle(HttpContext context, Exception ex)
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
