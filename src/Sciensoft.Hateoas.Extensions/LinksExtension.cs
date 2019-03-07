using Microsoft.Extensions.DependencyInjection;
using Sciensoft.Hateoas.Filters;
using System;

namespace Sciensoft.Hateoas.Extensions
{
	public static class LinksExtension
	{
		private static readonly LinksBuilder linksOptions = new LinksBuilder();

		public static IServiceCollection AddLinks(
			this IServiceCollection services,
			Action<LinksBuilder> policySetup = null)
		{
			services.AddMvc(setup =>
			{
				setup.Filters.Add<LocationUriResultFilter>();
				setup.Filters.Add<HateoasResultFilter>();
			});

			policySetup?.Invoke(linksOptions);

			return services;
		}

		public static IServiceCollection AddLinks(this IMvcBuilder mvcBuilder, Action<LinksBuilder> policySetup = null)
			=> mvcBuilder.Services.AddLinks(policySetup);
	}
}
