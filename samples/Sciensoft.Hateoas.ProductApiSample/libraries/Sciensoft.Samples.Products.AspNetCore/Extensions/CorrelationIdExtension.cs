using Microsoft.Extensions.DependencyInjection;
using Sciensoft.Samples.Products.AspNetCore.Providers;

namespace Sciensoft.Samples.Products.AspNetCore.Extensions
{
    public static class CorrelationIdExtension
    {
        public static IServiceCollection AddCorrelationAndCausation(this IServiceCollection services)
        {
            return services
                .AddScoped(s => CorrelationProvider.Create())
                .AddScoped<CausationProvider>();
        }
    }
}
