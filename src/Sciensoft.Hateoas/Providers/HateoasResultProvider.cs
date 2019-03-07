using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sciensoft.Hateoas.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sciensoft.Hateoas.Providers
{
	internal class HateoasResultProvider : IHateoasResultProvider
	{
		readonly IOptions<MvcJsonOptions> _jsonOptions;
		readonly IHateoasUriProvider _uriProvider;

		public HateoasResultProvider(
			IOptions<MvcJsonOptions> jsonOptions,
			IHateoasUriProvider uriProvider)
		{
			_jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
			_uriProvider = uriProvider ?? throw new ArgumentNullException(nameof(uriProvider));
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
			var json = JObject.Parse(rawPayload);
			json.Add("links", JToken.FromObject(Links, new JsonSerializer
			{
				ContractResolver = _jsonOptions.Value.SerializerSettings.ContractResolver,
				NullValueHandling = _jsonOptions.Value.SerializerSettings.NullValueHandling
			}));

			var content = new JsonResult(json, _jsonOptions.Value.SerializerSettings)
			{
				// TODO : Add support to Content Negotiation Content-Type
				ContentType = "application/hateoas+json"
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
