using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sciensoft.Samples.Products.AspNetCore.Abstractions;
using Sciensoft.Samples.Products.AspNetCore.Logging;
using Sciensoft.Samples.Products.AspNetCore.Middlewares;

namespace Sciensoft.Samples.Products.AspNetCore.Extensions
{
    public static class GlobalExceptionHandlerExtension
    {
        public static IServiceCollection AddGlobalExceptionHandler<THandler>(this IServiceCollection services)
            where THandler : class, IExceptionHandler
        {
            return services
                .AddGlobalExceptionHandler()
                .AddTransient<IExceptionHandler, THandler>();
        }

        public static IServiceCollection AddGlobalExceptionHandler(this IServiceCollection services)
        {
            return services
                .AddCoreLogger()
                .AddTransient<GlobalExceptionHandlerMiddleware>()
                .AddTransient<IExceptionHandlerLogger, ExceptionHandlerLogger>();
        }

        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder appBuilder)
            => appBuilder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}
