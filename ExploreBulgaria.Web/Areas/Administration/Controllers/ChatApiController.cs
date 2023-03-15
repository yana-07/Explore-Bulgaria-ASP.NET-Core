using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Services.Data.Administration;
using ExploreBulgaria.Web.ViewModels.Administration;
using ExploreBulgaria.Web.ViewModels.Chat;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Areas.Administration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatApiController : ControllerBase
    {
        private readonly IAdminService adminService;
        private readonly IChatService chatService;

        public ChatApiController(
            IAdminService adminService,
            IChatService chatService)
        {
            this.adminService = adminService;
            this.chatService = chatService;
        }

        [HttpPost("clearNotification")]
        public async Task<IActionResult> Index(
            ClearAdminNotificationInputModel model)
        {
            await adminService.ClearAdminNotification(model.Group);
            return NoContent();
        }

        [HttpPost("clearMessages")]
        public async Task<IActionResult> ClearMessages(
            ClearChatMessageViewModel model)
        {
            await chatService.ClearMessages(model);
            return NoContent();
        }
    }
}
