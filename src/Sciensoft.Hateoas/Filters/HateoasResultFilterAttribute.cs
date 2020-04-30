using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Sciensoft.Hateoas.Providers;
using Sciensoft.Hateoas.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sciensoft.Hateoas.Filters
{
	internal class HateoasResultFilterAttribute : ResultFilterAttribute
	{
		private readonly ILogger<HateoasResultFilterAttribute> _logger;
		private readonly IHateoasResultProvider _resultProvider;
		private readonly JsonSerializerOptions _jsonOptions;

		public HateoasResultFilterAttribute(
			ILogger<HateoasResultFilterAttribute> logger,
			IHateoasResultProvider resultProvider,
			JsonSerializerOptions jsonOptions = null)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_resultProvider = resultProvider ?? throw new ArgumentNullException(nameof(resultProvider));
			_jsonOptions = jsonOptions;
		}

		public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
		{
			try
			{
				if (context.Result is ObjectResult result)
				{
					var policies = GetFilteredPolicies(InMemoryPolicyRepository.InMemoryPolicies, result);

					var resultType = result.Value.GetType();
					if (policies.Any() && !(typeof(IEnumerable<object>)).IsAssignableFrom(resultType))
					{
						var rawJson = JsonSerializer.Serialize(result.Value, _jsonOptions);

						foreach (var policy in policies.Where(p => p != null))
						{
							var lambdaResult = GetLambdaResult(policy.Expression, result.Value, policy.Type);
							await _resultProvider.AddPolicyResultAsync(policy, lambdaResult).ConfigureAwait(false);
						}

						context.Result = await _resultProvider.GetContentResultAsync(rawJson).ConfigureAwait(false);
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Something went wrong while processing the link generation.");
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
				config.CreateMap(sourcePayload.GetType(), targetPayloadType);
			});

			mapperConfig
				.CreateMapper()
				.Map(sourcePayload, targetPayload);

			return Expression.Lambda(body, parameter).Compile().DynamicInvoke(targetPayload);
		}

		private IList<InMemoryPolicyRepository.Policy> GetFilteredPolicies(IList<InMemoryPolicyRepository.Policy> policies, ObjectResult result)
			=> policies.Where(p => p.Type == result.DeclaredType || p.Type == result.Value.GetType()).ToList();
	}
}
