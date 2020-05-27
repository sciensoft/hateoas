using System;

namespace Sciensoft.Hateoas
{
	/// <summary>
	/// Allows link policy configuration.
	/// </summary>
	public sealed class LinkBuilder : ILinkBuilder
	{
		/// <inheritdoc/>
		public LinkBuilder AddPolicy<T>(Action<PolicyBuilder<T>> modelSetup)
			where T : class
		{
			var policy = new PolicyBuilder<T>();

			modelSetup?.Invoke(policy);

			return this;
		}
	}
}
