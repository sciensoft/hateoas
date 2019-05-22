using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sciensoft.Samples.Products.AspNetCore.Logging;
using System;

namespace Sciensoft.Samples.Products.AspNetCore.Extensions
{
    public static class CoreLoggerExtension
    {
        public static IWebHostBuilder UseCoreLogger(
            this IWebHostBuilder hostBuilder,
            Action<ILoggingBuilder> config)
            => hostBuilder
                .ConfigureServices(services => services.Add(new ServiceDescriptor(typeof(ICoreLogger), typeof(CoreLogger), ServiceLifetime.Transient)))
                .ConfigureLogging(config);

        public static IWebHostBuilder UseCoreLogger(this IWebHostBuilder hostBuilder)
        {
            return hostBuilder.UseCoreLogger(config =>
            {
                config.ClearProviders();
                config.AddConsole();
            });
        }

        internal static IServiceCollection AddCoreLogger(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            if (serviceProvider.GetService<ICoreLogger>() == null)
            {
                var loggerBuilder = serviceProvider.GetService<ILoggingBuilder>();
                var loggerProvider = serviceProvider.GetService<ILoggerProvider>();
                if (loggerBuilder == null)
                {
                    var logger = loggerProvider.CreateLogger(nameof(ICoreLogger));
                    services.AddSingleton(s => logger);
                }

                services.AddTransient<ICoreLogger, CoreLogger>();
            }

            return services;
        }
    }
}
