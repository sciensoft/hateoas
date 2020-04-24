using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Sciensoft.Hateoas.Filters;
using Sciensoft.Hateoas.Providers;
using System;

namespace Sciensoft.Hateoas.Extensions
{
	public static class LinksExtension
	{
		private static readonly LinksBuilder linksBuilder = new LinksBuilder();

		public static IServiceCollection AddLinks(
			this IServiceCollection services,
			Action<LinksBuilder> policySetup = null)
		{
			services.AddSingleton<IRouteCollection, RouteCollection>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

			services.AddTransient<HateoasProxyUriProvider, HateoasProxyUriProvider>();
			services.AddTransient<IHateoasResultProvider, HateoasResultProvider>();

			services
				.AddMvcCore(setup =>
				{
					setup.Filters.Add<LocationUriResultFilter>();
					setup.Filters.Add<HateoasResultFilter>();
				});

			//.AddJsonOptions(options =>
			//{
			//	options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver
			//	{
			//		NamingStrategy = new SnakeCaseNamingStrategy()
			//	};
			//	options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
			//	options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
			//});

			policySetup?.Invoke(linksBuilder);

			return services;
		}

		public static IServiceCollection AddLinks(this IMvcBuilder mvcBuilder, Action<LinksBuilder> policySetup = null)
			=> mvcBuilder.Services.AddLinks(policySetup);
	}
}
