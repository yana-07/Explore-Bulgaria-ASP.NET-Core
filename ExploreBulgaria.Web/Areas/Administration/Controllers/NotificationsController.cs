using ExploreBulgaria.Services.Data.Administration;
using ExploreBulgaria.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Areas.Administration.Controllers
{
    public class NotificationsController : BaseController
    {
        private readonly IAdminService adminService;

        public NotificationsController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await adminService
                .GetAdminNotifications(User.VisitorId());

            return View(model);
        }

        public IActionResult Chat(string group)
        {
            ViewData["Group"] = group;
            ViewData["UserIdentifier"] = User.Id();
            return View();
        }
    }
}
