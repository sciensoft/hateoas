using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Sciensoft.Samples.Products.AspNetCore.Extensions;

namespace Sciensoft.Samples.Products.Web.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
                //.UseCoreLogger();
    }
}
