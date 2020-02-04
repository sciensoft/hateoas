using System;

namespace Sciensoft.Hateoas
{
	public sealed class LinksBuilder : ILinksBuilder
	{
		public LinksBuilder AddPolicy<T>(Action<PolicyBuilder<T>> modelSetup)
			where T : class
		{
			var policy = new PolicyBuilder<T>();

			modelSetup?.Invoke(policy);

			return this;
		}
	}
}
