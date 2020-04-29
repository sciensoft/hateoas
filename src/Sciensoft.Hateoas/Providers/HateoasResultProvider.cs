using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Sciensoft.Hateoas.Repositories;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sciensoft.Hateoas.Providers
{
	internal class HateoasResultProvider : IHateoasResultProvider
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly JsonSerializerOptions _jsonOptions;

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

		public IList<object> Links { get; } = new List<object>();

		public async Task AddPolicyResultAsync(InMemoryPolicyRepository.Policy policy, object result)
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

			Links.Add(new
			{
				Method = endpoint.Item1,
				Uri = endpoint.Item2,
				Relation = policy.Name,
			});

			await Task.CompletedTask.ConfigureAwait(false);
		}

		public async Task<IActionResult> GetContentResultAsync(string rawPayload)
		{
			var finalPayload = JsonSerializer.Deserialize<Dictionary<string, object>>(rawPayload, _jsonOptions);
			finalPayload.Add("links", Links);

			var content = new JsonResult(finalPayload)
			{
				// TODO : Add support to Content Negotiation Content-Type
				//ContentType = "application/hateoas+json"
			};

			return await Task.FromResult(content).ConfigureAwait(false);
		}
	}

	internal interface IHateoasResultProvider
	{
		Task AddPolicyResultAsync(InMemoryPolicyRepository.Policy policy, object result);

		Task<IActionResult> GetContentResultAsync(string rawPayload);
	}
}
