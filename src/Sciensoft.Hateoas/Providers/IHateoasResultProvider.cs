using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Sciensoft.Hateoas.Providers
{
	internal interface IHateoasResultProvider
	{
		Task<IActionResult> GetContentResultAsync(ObjectResult result);
	}
}
