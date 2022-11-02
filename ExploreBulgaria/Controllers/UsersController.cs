using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.Infrastructure;
using ExploreBulgaria.Web.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            var model = new RegisterViewModel();

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await this.usersService.SignUpAsync(model);

            if (result.Succeeded)
            {
                return this.RedirectTo<UsersController>(c => c.Login());
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            var model = new LoginViewModel();

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await usersService.SignInAsync(model);

            if (result.Succeeded)
            {
                // TODO: change redirect strategy
                return this.RedirectTo<HomeController>(c => c.Index());
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return View(model);
        }

        public async Task<IActionResult> Logout(string returnUrl)
        {
            await this.usersService.SignOutAsync();

            return Redirect(returnUrl);
        }
    }
}
