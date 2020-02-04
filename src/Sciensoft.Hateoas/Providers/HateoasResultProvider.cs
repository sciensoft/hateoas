using Microsoft.AspNetCore.Mvc;
using Sciensoft.Hateoas.Repository;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sciensoft.Hateoas.Providers
{
	internal class HateoasResultProvider : IHateoasResultProvider
	{
		readonly JsonSerializerOptions _jsonOptions;
		readonly IHateoasUriProvider _uriProvider;

		public HateoasResultProvider(
			IHateoasUriProvider uriProvider,
			JsonSerializerOptions jsonOptions = null)
		{
			_uriProvider = uriProvider ?? throw new ArgumentNullException(nameof(uriProvider));
			_jsonOptions = jsonOptions ?? new JsonSerializerOptions
			{
				IgnoreNullValues = true
			};
		}

		public IList<object> Links { get; } = new List<object>();

		public async Task AddPolicyAsync(PolicyInMemoryRepository.Policy policy, object result)
		{
			/* TODO : Links should be composed of
			 * 
			 * {
			 *   "method": "GET|PUT|POST|DELETE|HEAD|OPTION|...",
			 *   "uri": "http://127.0.0.1:8080/api/sample/20",
			 *   "relation": "self|new|update|delete|custom_{name}",
			 * }
            */

			await Task.Run(() => Links.Add(new
			{
				policy.Method,
				Uri = _uriProvider.GenerateUri(policy, result),
				Relation = policy.Name,
			})).ConfigureAwait(false);
		}

		public async Task<IActionResult> GetContentResultAsync(string rawPayload)
		{
			var finalPayload = JsonSerializer.Deserialize<Dictionary<string, object>>(rawPayload, _jsonOptions);
			finalPayload.Add("links", Links); // TODO : Remove null items

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
		Task AddPolicyAsync(PolicyInMemoryRepository.Policy policy, object result);

		Task<IActionResult> GetContentResultAsync(string rawPayload);
	}
}
