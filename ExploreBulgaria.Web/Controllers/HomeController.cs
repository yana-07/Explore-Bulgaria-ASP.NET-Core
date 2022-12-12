using ExploreBulgaria.Web.ViewModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static ExploreBulgaria.Services.Constants.GlobalConstants;

namespace ExploreBulgaria.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger;
        }

        public IActionResult Index()
        {
            if (User.IsInRole(AdministratorRoleName))
            {
                return RedirectToAction(
                    nameof(Areas.Administration.Controllers.HomeController.Index),
                    "Home",
                    new { area = AdministrationAreaName });
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 30, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var feature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var traceIdentifier = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            logger.LogError(feature?.Error, "TraceIdentifier: {0}", traceIdentifier);

            return View(new ErrorViewModel { RequestId = traceIdentifier });
        }

        public IActionResult StatusCodeError(int statusCode)
        {
            if (statusCode == 404)
            {
                return View("404");
            }

            return View();
        }
    }
}