using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Areas.Administration.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
