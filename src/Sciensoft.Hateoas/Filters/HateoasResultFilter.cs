using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Sciensoft.Hateoas.Providers;
using Sciensoft.Hateoas.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sciensoft.Hateoas.Filters
{
	internal class HateoasResultFilter : ResultFilterAttribute
	{
		readonly IHateoasResultProvider _resultProvider;
		readonly JsonSerializerOptions _jsonOptions;

		public HateoasResultFilter(
			IHateoasResultProvider resultProvider,
			JsonSerializerOptions jsonOptions = null)
		{
			_resultProvider = resultProvider ?? throw new ArgumentNullException(nameof(resultProvider));
			_jsonOptions = jsonOptions;
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
						var rawJson = JsonSerializer.Serialize(result.Value, _jsonOptions);

						foreach (var policy in policies)
						{
							if (policy != null)
							{
								var payload = JsonSerializer.Deserialize(rawJson, policy.Type);
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

			if (lambdaExpression == null)
				return null;

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
