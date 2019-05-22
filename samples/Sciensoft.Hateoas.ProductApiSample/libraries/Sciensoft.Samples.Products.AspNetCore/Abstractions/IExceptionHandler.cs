using Microsoft.AspNetCore.Http;
using System;

namespace Sciensoft.Samples.Products.AspNetCore.Abstractions
{
    public interface IExceptionHandler
    {
        void Handle(HttpContext context, Exception exception);
    }
}
