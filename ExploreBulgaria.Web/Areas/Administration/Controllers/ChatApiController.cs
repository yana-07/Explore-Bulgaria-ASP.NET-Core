using ExploreBulgaria.Services.Data.Administration;
using ExploreBulgaria.Web.ViewModels.Administration;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Areas.Administration.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatApiController : ControllerBase
    {
        private readonly IAdminService adminService;

        public ChatApiController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpPost("clearNotification")]
        public async Task<IActionResult> Index(
            ClearAdminNotificationInputModel model)
        {
            await adminService.ClearAdminNotification(model.Group);
            return NoContent();
        }
    }
}
