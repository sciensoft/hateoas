using Microsoft.AspNetCore.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Sciensoft.Hateoas.WebSample
{
	public class Program
	{
		public static void Main(string[] args)
		{
			if (args.Any(a => a.Equals("--debug")))
				Debugger.Launch();

			new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseStartup<Startup>()
				.Build()
				.Run();
		}
	}
}
