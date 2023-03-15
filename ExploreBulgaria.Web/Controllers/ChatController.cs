using ExploreBulgaria.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Controllers
{
    public class ChatController : BaseController
    {
        public IActionResult Index()
        {
            ViewData["UserIdentifier"] = User.VisitorId();
            return View();
        }
    }
}
