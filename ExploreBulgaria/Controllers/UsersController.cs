using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.Common;
using ExploreBulgaria.Web.Infrastructure;
using ExploreBulgaria.Web.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IWebHostEnvironment environment;

        public UsersController(
            IUsersService usersService,
            IWebHostEnvironment environment)
        {
            this.usersService = usersService;
            this.environment = environment;
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

            var (result, user) = this.usersService.SignUpAsync(model);

            var resultAwaited = await result;

            if (resultAwaited.Succeeded)
            {
                await usersService.AddFirstNameClaimAsync(user!);

                await usersService.AddLastNameClaimAsync(user!);

                return this.RedirectTo<UsersController>(c => c.Login());
            }

            foreach (var error in resultAwaited.Errors)
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

            ModelState.AddModelError(string.Empty, "Неуспешен опит.");

            return View(model);
        }

        public async Task<IActionResult> Logout(string returnUrl)
        {
            await this.usersService.SignOutAsync();

            return Redirect(returnUrl);
        }

        public async Task<IActionResult> Profile()
        {
            var model = await this.usersService.GetProfileAsync<UserProfileViewModel>(User.Id());

            return View(model);
        }

        public async Task<IActionResult> EditProfile()
        {
            var model = await this.usersService.GetProfileAsync<EditUserProfileInputModel>(User.Id());

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditUserProfileInputModel model)
        {
            if (!await usersService.UserNameAvailable(model.UserName))
            {
                model.AvatarUrl = model.AvatarUrlPreliminary;

                ModelState.AddModelError(nameof(model.UserName), "Потребителското име е заето.");
            }

            if (!await usersService.EmailAvailable(model.Email))
            {
                model.AvatarUrl = model.AvatarUrlPreliminary;

                ModelState.AddModelError(nameof(model.Email), "Имейлът е зает.");
            }

            if (!ModelState.IsValid)
            {
                model.AvatarUrl = model.AvatarUrlPreliminary;

                return View(model);
            }

            var path = environment.WebRootPath;
            // TODO: Save changes

            return RedirectToAction(nameof(Profile));
        }
    }
}
