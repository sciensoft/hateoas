using System;

namespace Sciensoft.Hateoas
{
	/// <summary>
	/// An interface for configuring link policy.
	/// </summary>
	public interface ILinkBuilder
	{
		/// <summary>
		/// Adds policy configuration to a generic view-model type.
		/// </summary>
		/// <typeparam name="T">View-model type for policy configuration.</typeparam>
		/// <param name="modelSetup">An <see cref="Action{PolicyBuilder}"/> to configure a policy for the generic type.</param>
		/// <returns>Link builder configured.</returns>
		LinkBuilder AddPolicy<T>(Action<PolicyBuilder<T>> modelSetup) where T : class;
	}
}
