using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Sciensoft.Hateoas.Providers;
using Sciensoft.Hateoas.Repository;

namespace Sciensoft.Hateoas.Filters
{
	internal class HateoasResultFilter : ResultFilterAttribute
	{
		readonly IOptions<MvcJsonOptions> _jsonOptions;
		readonly IHateoasResultProvider _resultProvider;
		
		public HateoasResultFilter(
			IOptions<MvcJsonOptions> jsonOptions,
			IHateoasResultProvider resultProvider)
		{
			_jsonOptions = jsonOptions ?? throw new ArgumentNullException(nameof(jsonOptions));
			_resultProvider = resultProvider ?? throw new ArgumentNullException(nameof(resultProvider));
		}

		public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
		{
			try
			{
				if (context.Result is ObjectResult result)
				{
					var policies = GetFilteredPolicies(PolicyInMemoryRepository.LinksPolicyInMemory, result);

					if (policies.Any())
					{
						var rawJson = JsonConvert.SerializeObject(result.Value, _jsonOptions.Value.SerializerSettings);

						foreach (var policy in policies)
						{
							if (policy != null)
							{
								var payload = JsonConvert.DeserializeObject(rawJson, policy.Type);
								var lambdaResult = GetLambdaResult(policy.Expression, payload, policy.Type);

								await _resultProvider.AddPolicyAsync(policy, lambdaResult).ConfigureAwait(false);
							}
						}

						context.Result = await _resultProvider.GetContentResultAsync(rawJson).ConfigureAwait(false);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
				// TODO : Log exception
			}

			await base.OnResultExecutionAsync(context, next).ConfigureAwait(false);
		}

		private object GetLambdaResult(Expression expression, object sourcePayload, Type targetPayloadType)
		{
			var lambdaExpression = (expression as LambdaExpression);

			var parameter = lambdaExpression.Parameters[0];
			var body = lambdaExpression.Body;

			var targetPayload = Activator.CreateInstance(targetPayloadType);

			var mapperConfig = new MapperConfiguration(config =>
			{
				//config.CreateMissingTypeMaps = true;
				config.CreateMap(sourcePayload.GetType(), targetPayloadType);
			});

			mapperConfig
				.CreateMapper()
				.Map(sourcePayload, targetPayload);

			return Expression.Lambda(body, parameter).Compile().DynamicInvoke(targetPayload);
		}

		private IList<PolicyInMemoryRepository.Policy> GetFilteredPolicies(IList<PolicyInMemoryRepository.Policy> policies, ObjectResult result)
			=> policies.Where(p => p.Type == result.DeclaredType || p.Type == result.Value.GetType()).ToList();
	}
}
