using FluentValidation.AspNetCore;
using Markdig;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Sciensoft.Samples.Products.Web.Application.Logging;
using Sciensoft.Samples.Products.Web.Presentation.Clients;
using Sciensoft.Samples.Products.Web.Presentation.Middlewares;
using Sciensoft.Samples.Products.Api.Infrastructure.Abstractions;
using Sciensoft.Samples.Products.Api.Infrastructure.Repository;
using Sciensoft.Samples.Products.AspNetCore.Extensions;
using Sciensoft.Samples.Products.Web.Application.Adapters;
using Sciensoft.Samples.Products.Web.Application.Providers;
using Sciensoft.Samples.Products.Web.Application.Services;

namespace Sciensoft.Samples.Products.Web.Presentation
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorrelationAndCausation();
            services.AddGlobalExceptionHandler<GlobalExceptionHandler>();

            services.AddTransient<MarkdownPipelineBuilder>();
            services.AddTransient<MarkdownConverterProvider, MarkdigConverter>();
            services.AddTransient(typeof(IDataAdapter<string>), typeof(HtmlPageDataAdapter));
            services.AddTransient(typeof(FileSystemReadRepository<string>));

            services.AddTransient<ProductClient>();
            services.AddTransient<IHomeOrchestrator, HomeOrchestrator>();
            services.AddTransient<IHomeOrchestratorLogger, HomeOrchestratorLogger>();

            services
                .AddMvc()
                .AddSessionStateTempDataProvider()
                .AddFluentValidation();

            services.AddSession();
        }

        public void Configure(IApplicationBuilder appBuilder, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                appBuilder.UseDeveloperExceptionPage();
            }

            appBuilder
                .UseStaticFiles()
                .UseSession()
                .UseGlobalExceptionHandler()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "InternalServerError", 
                        template: "/error", 
                        defaults: new { controller = "Error", action = "InternalServerError500" });

                    routes.MapRoute("Default", "{controller=Home}/{action=About}/{code?}");

                    routes.MapRoute(
                        name: "Catch-All",
                        template: "{*url}",
                        defaults: new { controller = "Error", action = "NotFound404" });
                });
        }
    }
}
