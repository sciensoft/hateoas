using Microsoft.AspNetCore.Http;
using Sciensoft.Hateoas.Constants;
using Sciensoft.Hateoas.Exceptions;
using Sciensoft.Hateoas.Repository;
using System;
using System.Linq.Expressions;

namespace Sciensoft.Hateoas
{
	// TODO : Create Unit Tests
	public sealed class PolicyBuilder<T>
		where T : class
	{
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

		public PolicyBuilder<T> AddCustom(Expression<Func<T, object>> expression, string linkKey, string template = null, string method = null, string message = null)
		{
			if (string.IsNullOrWhiteSpace(linkKey))
			{
				throw new InvalidPolicyConfigurationException($"Custom Policy requires '{nameof(linkKey)}'.");
			}

			PolicyInMemoryRepository.LinksPolicyInMemory.Add(
				new PolicyInMemoryRepository.TemplatePolicy(typeof(T), expression, linkKey)
				{
					Template = template,
					Method = method,
					Message = message
				});

			return this;
		}
	}
}