using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Sciensoft.Samples.Products.Web.Presentation.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// View for 404 error, <see cref="HttpStatusCode.NotFound"/>
        /// </summary>
        /// <returns></returns>
        public IActionResult NotFound404()
        {
            return View("404");
        }

        /// <summary>
        /// View for 500 error, <see cref="HttpStatusCode.InternalServerError"/>
        /// </summary>
        /// <returns></returns>
        public IActionResult InternalServerError500()
        {
            return View("500");
        }
    }
}
