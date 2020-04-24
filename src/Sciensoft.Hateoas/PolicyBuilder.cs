using Microsoft.AspNetCore.Http;
using Sciensoft.Hateoas.Constants;
using Sciensoft.Hateoas.Exceptions;
using Sciensoft.Hateoas.Repository;
using System;
using System.Linq.Expressions;

namespace Sciensoft.Hateoas
{
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
			PolicyInMemoryRepository.LinksPolicyInMemory.Add(
				new PolicyInMemoryRepository.TemplatePolicy(typeof(T), expression, PolicyConstants.Self)
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
				throw new InvalidPolicyConfigurationException($"Routed Policy requires '{nameof(routeName)}'.");
			}

			PolicyInMemoryRepository.LinksPolicyInMemory.Add(
				new PolicyInMemoryRepository.RoutePolicy(typeof(T), expression, routeName)
				{
					Method = method,
					Message = message
				});

			return this;
		}

		/// <summary>
		/// Adds query based URI to the resource representation.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="parameterName"></param>
		/// <param name="method"></param>
		/// <param name="message"></param>
		/// <returns></returns>
		public PolicyBuilder<T> AddQuery(Expression<Func<T, object>> expression, string parameterName, string method = null, string message = null)
		{
			throw new NotImplementedException("Feature not implemented yet.");
		}

		/// <summary>
		/// Adds custom defined URI to the resource representation, e.g., would be pointing to an external URI.
		/// </summary>
		/// <param name="expression">Path expression, e.g. @"https://my-external-uri.com/api/resource/{@id}"</param>
		/// <param name="linkKey">Link identifier</param>
		/// <param name="method">Resource method as per HTTP methods, e.g. GET/HEAD/POST, for more visit <see cref="https://tools.ietf.org/html/rfc7231#section-4" /></param>
		/// <param name="message">Usage message</param>
		/// <returns></returns>
		public PolicyBuilder<T> AddCustomPath(Expression<Func<T, object>> expression, string linkKey, string method = null, string message = null)
		{
			if (string.IsNullOrWhiteSpace(linkKey))
			{
				throw new InvalidPolicyConfigurationException($"Custom Policy requires '{nameof(linkKey)}'.");
			}

			PolicyInMemoryRepository.LinksPolicyInMemory.Add(
				new PolicyInMemoryRepository.TemplatePolicy(typeof(T), expression, linkKey)
				{
					Template = null,
					Method = method,
					Message = message
				});

			return this;
		}
	}
}