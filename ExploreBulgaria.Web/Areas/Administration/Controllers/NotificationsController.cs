using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Services.Data.Administration;
using ExploreBulgaria.Web.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Areas.Administration.Controllers
{
    public class NotificationsController : BaseController
    {
        private readonly IAdminService adminService;
        private readonly IChatService chatService;
        private readonly IVisitorsService visitorsService;

        public NotificationsController(
            IAdminService adminService,
            IChatService chatService,
            IVisitorsService visitorsService)
        {
            this.adminService = adminService;
            this.chatService = chatService;
            this.visitorsService = visitorsService;
        }

        public async Task<IActionResult> Index()
        {
            var model = await adminService
                .GetAdminNotifications(User.VisitorId());

            return View(model);
        }

        public async Task<IActionResult> Chat(string group, string fromVisitorId)
        {
            ViewData["Group"] = group;
            ViewData["UserIdentifier"] = User.VisitorId();

            var messages = await chatService.GetMessages(fromVisitorId, User.VisitorId());

            return View(messages);
        }
    }
}
