using Microsoft.AspNetCore.Mvc;
using Sciensoft.Hateoas.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sciensoft.Hateoas.Providers
{
	internal class HateoasResultProvider : IHateoasResultProvider
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly JsonSerializerOptions _jsonOptions;

		private readonly IList<object> _links = new List<object>();

		public HateoasResultProvider(
			IServiceProvider serviceProvider,
			JsonSerializerOptions jsonOptions = null)
		{
			_serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
			_jsonOptions = jsonOptions ?? new JsonSerializerOptions
			{
				IgnoreNullValues = true
			};
		}

		public async Task<IActionResult> GetContentResultAsync(ObjectResult result)
		{
			var policies = GetFilteredPolicies(result);
			var resultType = result.Value.GetType();

			if (!policies.Any() && (typeof(IEnumerable<object>)).IsAssignableFrom(resultType))
			{
				return null;
			}

			foreach (var policy in policies.Where(p => p != null))
			{
				var lambdaResult = GetLambdaResult(policy.Expression, result.Value);
				await AddPolicyResultAsync(policy, lambdaResult).ConfigureAwait(false);
			}

			// TODO : Remove this serialization and Use JsonDocument Parse
			var rawJson = JsonSerializer.Serialize(result.Value, _jsonOptions);
			var finalPayload = JsonSerializer.Deserialize<Dictionary<string, object>>(rawJson, _jsonOptions);
			finalPayload.Add("links", _links);

			var content = new JsonResult(finalPayload);

			return await Task.FromResult(content).ConfigureAwait(false);
		}

		private async Task AddPolicyResultAsync(InMemoryPolicyRepository.Policy policy, object result)
		{
			var uriProviderType = typeof(HateoasUriProvider<>).MakeGenericType(policy.GetType());
			var uriProvider = _serviceProvider.GetService(uriProviderType);

			var endpoint = (dynamic)uriProvider.GetType()
				.InvokeMember(
					nameof(HateoasUriProvider<InMemoryPolicyRepository.Policy>.GenerateEndpoint),
					BindingFlags.InvokeMethod,
					null,
					uriProvider,
					new[] { policy, result });

			_links.Add(new
			{
				Method = endpoint.Item1,
				Uri = endpoint.Item2,
				Relation = policy.Name,
				policy.Message
			});

			await Task.CompletedTask.ConfigureAwait(false);
		}

		private object GetLambdaResult(Expression expression, object sourcePayload)
		{
			var lambdaExpression = (expression as LambdaExpression);

			if (lambdaExpression == null)
				return null;

			var body = lambdaExpression.Body;
			var parameter = lambdaExpression.Parameters[0];

			return Expression.Lambda(body, parameter).Compile().DynamicInvoke(sourcePayload);
		}

		private IList<InMemoryPolicyRepository.Policy> GetFilteredPolicies(ObjectResult result)
			=> InMemoryPolicyRepository.InMemoryPolicies.Where(p => p.Type == result.DeclaredType || p.Type == result.Value.GetType()).ToList();
	}
}
