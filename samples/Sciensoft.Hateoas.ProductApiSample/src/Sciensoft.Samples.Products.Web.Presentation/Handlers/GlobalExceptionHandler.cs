using Microsoft.AspNetCore.Http;
using Sciensoft.Samples.Products.AspNetCore.Abstractions;
using System;

namespace Sciensoft.Samples.Products.Web.Presentation.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        const string ErrorRoute = "/error";

        public void Handle(HttpContext context, Exception ex)
        {
            if (!context.Request.Path.Value.Contains(ErrorRoute))
            {
                context.Response.Redirect(ErrorRoute);
            }
        }
    }
}