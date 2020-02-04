using System;

namespace Sciensoft.Hateoas
{
	public interface ILinksBuilder
	{
		LinksBuilder AddPolicy<T>(Action<PolicyBuilder<T>> modelSetup) where T : class;
	}
}
