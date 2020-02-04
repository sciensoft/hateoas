using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using System;
using System.Threading.Tasks;

namespace Sciensoft.Hateoas.Filters
{
    internal class LocationUriResultFilter : ResultFilterAttribute
    {
        public override async Task OnResultExecutionAsync(
            ResultExecutingContext context, 
            ResultExecutionDelegate next)
        {
            var httpContext = context.HttpContext;
            string location = httpContext.Response.Headers[HeaderNames.Location];

            if (!string.IsNullOrWhiteSpace(location))
            {
                if (!(new Uri(location, UriKind.RelativeOrAbsolute)).IsAbsoluteUri)
                {
                    var scheme = httpContext.Request.Scheme;
                    var host = httpContext.Request.Host;

                    bool startsWithSlash = location.StartsWith("/");

                    httpContext.Response.Headers.Remove(HeaderNames.Location);
                    httpContext.Response.Headers.Add(HeaderNames.Location, $"{scheme}://{host}{(startsWithSlash ? location : "/" + location)}");
                }
            }
            else
            {
                await base.OnResultExecutionAsync(context, next);
            }
        }
    }
}