using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Web.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersApiController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UsersApiController(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        [HttpPost("username")]
        public async Task<ActionResult<bool>> UserNameExists(UserInputModel input)
        {
            return await userManager.FindByNameAsync(input.UserName) != null;
        }

        [HttpPost("email")]
        public async Task<ActionResult<bool>> EmailExists(UserInputModel input)
        {
            return await userManager.FindByEmailAsync(input.Email) != null;
        }
    }
}
