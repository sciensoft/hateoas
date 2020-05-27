using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Sciensoft.Hateoas.Providers;
using System;
using System.Threading.Tasks;

namespace Sciensoft.Hateoas.Filters
{
	internal class HateoasResultFilterAttribute : ResultFilterAttribute
	{
		private readonly ILogger<HateoasResultFilterAttribute> _logger;
		private readonly IHateoasResultProvider _resultProvider;

		public HateoasResultFilterAttribute(
			ILogger<HateoasResultFilterAttribute> logger,
			IHateoasResultProvider resultProvider)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_resultProvider = resultProvider ?? throw new ArgumentNullException(nameof(resultProvider));
		}

		public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
		{
			try
			{
				if (_resultProvider.HasAnyPolicy(context.Result, out ObjectResult result))
				{
					var finalResult = await _resultProvider.GetContentResultAsync(result).ConfigureAwait(false);
					if (finalResult != null)
					{
						context.Result = finalResult;
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogWarning(ex, "Something went wrong while processing link generation.");
			}

			await base.OnResultExecutionAsync(context, next).ConfigureAwait(false);
		}
	}
}
