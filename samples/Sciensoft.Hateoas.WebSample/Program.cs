using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Sciensoft.Hateoas.WebSample
{
    public class Program
    {
        public static void Main(string[] args)
            => new WebHostBuilder()
                    .UseKestrel()
                    .UseContentRoot(Directory.GetCurrentDirectory())
                    .UseStartup<Startup>()
                    .Build()
                    .Run();
    }
}
