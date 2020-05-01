using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Sciensoft.Hateoas.WebSample
{
	static class Program
	{
		static async Task Main(string[] args)
		{
			if (args.Any(a => a.Equals("--debug")))
				Debugger.Launch();

			await CreateHostBuilder(args).Build().RunAsync();
		}

		static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.ConfigureKestrel(options =>
					{
						options.ListenAnyIP(6080);
					});
					webBuilder.UseStartup<Startup>();
				});
	}
}
