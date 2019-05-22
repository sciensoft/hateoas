using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sciensoft.Samples.Products.Api.Infrastructure.Repository;
using System;
using System.IO;

namespace Sciensoft.Samples.Products.Api.Infrastructure.EfMigrations
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        { /* Used for netcoreapp2.x to support Entity Framework migrations */ }

        public void Configure(IApplicationBuilder appBuilder)
        { /* Used for netcoreapp2.x to support Entity Framework migrations */ }
    }

    public class AuthenticationDbContextFactory : IDesignTimeDbContextFactory<ProductsDbContext>
    {
        public ProductsDbContext CreateDbContext(string[] args)
        {
            return Configure(args);
        }

        public static ProductsDbContext Configure(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            Console.WriteLine("Configuring SQLite Server Context.");

            var connectionString = configuration.GetConnectionString("Products.Sqlite");

            var optionsBuilder = new DbContextOptionsBuilder<ProductsDbContext>();
            optionsBuilder.UseSqlite(connectionString, sqlOptions => sqlOptions.MigrationsAssembly("Sciensoft.Samples.Products.Api.Infrastructure"));

            return new ProductsDbContext(optionsBuilder.Options);
        }
    }
}