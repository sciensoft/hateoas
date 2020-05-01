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
				.AddControllers()
				.AddLink(builder =>
				{
					builder
						.AddPolicy<BookViewModel>(model =>
						{
							model
								.AddSelf(m => m.Id, "This is a GET self link.")
								.AddRoute(m => m.Id, BookController.UpdateBookById)
								.AddRoute(m => m.Id, BookController.DeleteBookById)
								.AddCustomPath(m => m.Id, "Edit", method: HttpMethods.Post, message: "Edits resource")
								.AddCustomPath(m => $"/change/resource/state/?id={m.Id}", "ChangeResourceState", method: HttpMethods.Post, message: "Any operation in your resource.");
						});

					builder
						.AddPolicy<ArticleViewModel>(model =>
						{
							model
								.AddSelf(m => m.Id, "Self link.")
								.AddRoute(m => m.Id, ArticlesController.UpdateArticleById)
								.AddCustomPath(m => $"/api/[controller]/list", "List All", method: HttpMethods.Get);
						});

					builder
						.AddPolicy<AuthorViewModel>(model =>
						{
							model
								.AddSelf(m => m.Id, "Self link.")
								.AddRoute(m => m.Id, AuthorsController.UpdateAuthorById)
								.AddCustomPath(m => $"/api/[controller]", "List All", method: HttpMethods.Post);
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
			IWebHostEnvironment env)
		{
			if (env.EnvironmentName.Contains("Development"))
			{
				appBuilder.UseDeveloperExceptionPage();
			}

			appBuilder
				.UseRouting()
				//.UseAuthorization()
				.UseEndpoints(builder =>
				{
					builder.MapControllers();
					builder.MapControllerRoute("authors", "api/{controller}/{action}/{id?}");
				});
		}
	}
}
