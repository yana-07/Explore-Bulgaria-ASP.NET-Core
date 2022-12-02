using ExploreBulgaria.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using static ExploreBulgaria.Services.Common.Constants.GlobalConstants;

namespace ExploreBulgaria.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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