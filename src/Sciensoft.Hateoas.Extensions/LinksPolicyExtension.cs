using System;

namespace Sciensoft.Hateoas.Extensions
{
	public static class LinksPolicyExtension
	{
		public static LinksBuilder AddPolicy<T>(
			this LinksBuilder linkService,
			Action<PolicyBuilder<T>> modelSetup)
			where T : class
		{
			var policy = new PolicyBuilder<T>();

			modelSetup?.Invoke(policy);

			return linkService;
		}
	}
}
