using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ExploreBulgaria.Services.Common.Constants.GlobalConstants;

namespace ExploreBulgaria.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = AdministratorRoleName)]
    [Area("Administration")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
