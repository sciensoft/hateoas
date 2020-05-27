using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sciensoft.Hateoas.Providers
{
	internal interface IHateoasResultProvider
	{
		bool HasAnyPolicy(IActionResult actionResult, out ObjectResult objectResult);

		Task<IActionResult> GetContentResultAsync(ObjectResult result);
	}
}
