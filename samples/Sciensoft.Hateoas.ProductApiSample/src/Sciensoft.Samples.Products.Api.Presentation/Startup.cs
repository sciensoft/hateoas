using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sciensoft.Hateoas.Extensions;
using Sciensoft.Samples.Products.Api.Application;
using Sciensoft.Samples.Products.Api.Application.Logging;
using Sciensoft.Samples.Products.Api.Application.Services;
using Sciensoft.Samples.Products.Api.Infrastructure.Repository;
using Sciensoft.Samples.Products.Api.Presentation.Controllers;
using Sciensoft.Samples.Products.Api.Presentation.Middlewares;
using Sciensoft.Samples.Products.AspNetCore.Extensions;
using Sciensoft.Samples.Products.AspNetCore.Filters.Filters;
using Sciensoft.Samples.Products.ViewModels;
using Sciensoft.Samples.Products.ViewModels.Validations;
using Swashbuckle.AspNetCore.Swagger;
using System.Linq;

namespace Sciensoft.Samples.Products.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorrelationAndCausation();

            services.AddTransient<IValidator<Product.Create>, ProductCreateValidator>();
            services.AddTransient<IValidator<Product.Update>, ProductUpdateValidator>();
            services.AddTransient<IValidator<JsonPatchDocument<Product.Update>>, ProductPatchValidator>();

            services.AddTransient(s => MapperFactory.CreateMapper());
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IEntityTagHashProvider, EntityTagHashProvider>();
            services.AddTransient<IProductOrchestrator, ProductOrchestrator>();
            services.AddTransient<IProductOrchestratorLogger, ProductOrchestratorLogger>();

            services.AddDbContext<ProductsDbContext>(options =>
                options.UseSqlite(Configuration.GetConnectionString("Products.Sqlite"), sqlOptions =>
                    sqlOptions.MigrationsAssembly("Sciensoft.Samples.Products.Api.Infrastructure")));

            services.AddGlobalExceptionHandler<GlobalExceptionHandler>();

            services
                .AddMvc(options => options.Conventions.Add(new ContentNegotiationConvention()))
                .AddFluentValidation()
                .AddLinks(config =>
                {
                    config
                        .AddPolicy<Product.Output>(policyConfig =>
                        {
                            policyConfig.AddSelf(m => string.Empty);
                            policyConfig.AddCustom(m => string.Empty, ProductsController.PatchRoute, method: HttpMethods.Patch);
                            policyConfig.AddCustom(m => string.Empty, ProductsController.DeleteRoute, method: HttpMethods.Delete);
                        });
                });

            services
                .AddSwaggerGen(config =>
                {
                    config.SwaggerDoc("v1", new Info { Title = "SuitSupply Product API", Version = "v1" });
                    config.ResolveConflictingActions(description => description.First());
                });
        }

        public void Configure(IApplicationBuilder appBuilder, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                appBuilder.UseDeveloperExceptionPage();
            }

            appBuilder.UseMvc()
                .UseGlobalExceptionHandler();

            appBuilder
                .UseSwagger()
                .UseSwaggerUI(config => config.SwaggerEndpoint("/swagger/v1/swagger.json", "SuitSupply API V1"));
        }
    }
}
