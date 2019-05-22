using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sciensoft.Samples.Products.Web.Application.Services;
using System;

namespace Sciensoft.Samples.Products.Web.Presentation.Controllers
{
    public class HomeController : Controller
    {
        readonly IHomeOrchestrator _homeOrchestrator;

        public HomeController(IHomeOrchestrator homeOrchestrator, ILogger<HomeController> logger)
            => _homeOrchestrator = homeOrchestrator ?? throw new ArgumentNullException(nameof(homeOrchestrator));

        public IActionResult About()
        {
            var result = _homeOrchestrator.RetrieveAboutContent();

            return View(new HtmlString(result));
        }
    }
}
