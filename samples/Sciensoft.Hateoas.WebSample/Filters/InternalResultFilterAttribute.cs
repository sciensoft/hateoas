using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sciensoft.Hateoas.WebSample.Filters
{
	public class ModelTypeResultFilterAttribute : ResultFilterAttribute
	{
		public override void OnResultExecuting(ResultExecutingContext context)
		{
			// For test purpose
			context.HttpContext.Response.Headers.Add("Model-Type", (context.Result as ObjectResult).DeclaredType.Name);
		}
	}
}
