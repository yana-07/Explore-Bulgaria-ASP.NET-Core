using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.Common;
using ExploreBulgaria.Web.Infrastructure;
using ExploreBulgaria.Web.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUsersService usersService;
        private readonly IVisitorsService visitorsService;
        private readonly IWebHostEnvironment environment;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UsersController(
            IUsersService usersService,
            IVisitorsService visitorsService,
            IWebHostEnvironment environment,
            SignInManager<ApplicationUser> signInManager)
        {
            this.usersService = usersService;
            this.visitorsService = visitorsService;
            this.environment = environment;
            this.signInManager = signInManager;
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
                var visitorId = await visitorsService.CreateByUserId(user!.Id);

                await usersService.AddVisitorIdClaimAsync(user, visitorId);

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
        public async Task<IActionResult> Login()
        {
            var model = new LoginViewModel
            {
                ExternalLogins = (await signInManager
                      .GetExternalAuthenticationSchemesAsync()).ToList()
            };

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
            if (model.UserName != User.Identity?.Name && !await usersService.UserNameAvailable(model.UserName))
            {
                model.AvatarUrl = model.AvatarUrlPreliminary;

                ModelState.AddModelError(nameof(model.UserName), "Потребителското име е заето.");
            }

            if (model.Email != User.Email() && !await usersService.EmailAvailable(model.Email))
            {
                model.AvatarUrl = model.AvatarUrlPreliminary;

                ModelState.AddModelError(nameof(model.Email), "Имейлът е зает.");
            }

            if (!ModelState.IsValid)
            {
                model.AvatarUrl = model.AvatarUrlPreliminary;

                return View(model);
            }

            // TODO: Save changes
            await usersService.EditProfileAsync(model, User.Id(), $"{environment.WebRootPath}/images");

            await usersService.SignAutAndInAsync(User);

            return this.RedirectTo<HomeController>(c => c.Index());
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Users", new { returnUrl });
            var proprties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, proprties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (remoteError != null)
            {
                TempData["ErrorMessage"] = $"Error from external provider: {remoteError}";

                return RedirectToAction(nameof(Login), new { returnUrl });
            }

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                TempData["ErrorMessage"] = "Error loading external login information.";

                return RedirectToAction(nameof(Login), new { returnUrl });
            }

            var result = await signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (result.Succeeded)
            {
                //TODO: Add Claims
                return LocalRedirect(returnUrl);
            }

            if (result.IsLockedOut)
            {
                // TODO: lockout redirect
                return RedirectToPage("Identity/Account/Lockout");
            }
            else
            {
                return RedirectToAction(nameof(Register));
            }
        }
    }
}
