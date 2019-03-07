using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Sciensoft.Hateoas.Extensions;
using Sciensoft.Hateoas.WebSample.Controllers;
using Sciensoft.Hateoas.WebSample.Models;

namespace Sciensoft.Hateoas.WebSample
{
	public class Startup
	{
		/// <summary>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		/// </summary>
		/// <param name="services"></param>
		public void ConfigureServices(IServiceCollection services)
		{
			services
				.AddMvc()
				.AddLinks(policy =>
				{
					policy
						.AddPolicy<SampleViewModel>(model =>
						{
							model
								.AddSelf(m => m.Id, "This is a GET self link.")
								.AddCustom(m => m.Id, "Edit", method: HttpMethods.Post, message: "Edits resource")
								//.AddCustom(m => m.Id, "CustomOne", "/move/to/", HttpMethods.Post)
								.AddCustom(m => $"/move/resource/state/?id={m.Id}", "MoveResourceState", method: HttpMethods.Post, message: "Any operation in your resource.")
								.AddRoute(m => m.Id, SampleController.GetWithId);
						});
				});
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		/// </summary>
		/// <param name="appBuilder"></param>
		/// <param name="env"></param>
		public void Configure(
			IApplicationBuilder appBuilder,
			IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				appBuilder.UseDeveloperExceptionPage();
			}

			appBuilder.UseMvc();
		}
	}
}
