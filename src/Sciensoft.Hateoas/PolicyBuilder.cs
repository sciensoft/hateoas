using Microsoft.AspNetCore.Http;
using Sciensoft.Hateoas.Constants;
using Sciensoft.Hateoas.Exceptions;
using Sciensoft.Hateoas.Repositories;
using System;
using System.Linq.Expressions;

namespace Sciensoft.Hateoas
{
	/// <summary>
	/// An <see cref="PolicyBuilder{T}"/> to add the policies to.
	/// </summary>
	/// <typeparam name="T">An view-model object type to configure the policies to.</typeparam>
	public sealed class PolicyBuilder<T>
		where T : class
	{
		/// <summary>
		/// Adds self URI to the resource representation.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public PolicyBuilder<T> AddSelf(Expression<Func<T, object>> expression, string message = null)
		{
			InMemoryPolicyRepository.InMemoryPolicies.Add(
				new InMemoryPolicyRepository.SelfPolicy(typeof(T), expression, PolicyConstants.Self)
				{
					Method = HttpMethods.Get,
					Message = message
				});

			return this;
		}

		/// <summary>
		/// Adds route based URI to the resource representation.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="routeName"></param>
		/// <param name="method"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public PolicyBuilder<T> AddRoute(Expression<Func<T, object>> expression, string routeName, string method = null, string message = null)
		{
			if (string.IsNullOrWhiteSpace(routeName))
			{
				throw new InvalidPolicyConfigurationException($"Routed policy requires '{nameof(routeName)}'.");
			}

			InMemoryPolicyRepository.InMemoryPolicies.Add(
				new InMemoryPolicyRepository.RoutePolicy(typeof(T), expression, routeName)
				{
					Method = method,
					Message = message
				});

			return this;
		}

		/// <summary>
		/// Adds custom defined URI to the resource representation, e.g., would be pointing to an external URI.
		/// </summary>
		/// <remarks>
		/// It's possible to use tokens E.g. [controller], [action], [your-own] to match and replace by the route values.
		/// </remarks>
		/// <param name="expression">Path expression for link generation, e.g. 'https://my-external-uri.com/api/resource/{@id}'.</param>
		/// <param name="linkKey">Link identifier.</param>
		/// <param name="method">Resource method as per HTTP methods, e.g. GET/HEAD/POST, visit <inheritdoc path="https://tools.ietf.org/html/rfc7231#section-4"/> for more information.</param>
		/// <param name="message">Descriptive message for the link.</param>
		/// <returns></returns>
		public PolicyBuilder<T> AddCustomPath(Expression<Func<T, object>> expression, string linkKey, string method = null, string message = null)
		{
			if (string.IsNullOrWhiteSpace(linkKey))
			{
				throw new InvalidPolicyConfigurationException($"Custom Policy requires '{nameof(linkKey)}'.");
			}

			InMemoryPolicyRepository.InMemoryPolicies.Add(
				new InMemoryPolicyRepository.CustomPolicy(typeof(T), expression, linkKey)
				{
					Method = method,
					Message = message
				});

			return this;
		}
	}
}