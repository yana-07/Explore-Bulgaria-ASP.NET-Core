using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Web.ViewModels.Users;
using Microsoft.AspNetCore.Identity;

namespace ExploreBulgaria.Services.Data
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public UsersService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public Task<SignInResult> SignInAsync(LoginViewModel model)
        {
            var user = userManager.FindByEmailAsync(model.Email).GetAwaiter().GetResult();

            if (user == null)
            {
                return Task.FromResult(SignInResult.Failed);
            }

            return signInManager.PasswordSignInAsync(user, model.Password, false, false);
        }

        public Task<IdentityResult> SignUpAsync(RegisterViewModel model)
        {
            var emailExists = userManager.FindByEmailAsync(model.Email).GetAwaiter().GetResult() != null;
            var usernameExists = userManager.FindByNameAsync(model.UserName).GetAwaiter().GetResult() != null;

            if (emailExists)
            {
                var error = new IdentityError
                {
                    Description = "An user with this email address already exists."
                };

                return Task.FromResult(IdentityResult.Failed(error));
            }

            if (usernameExists)
            {
                var error = new IdentityError
                {
                    Description = "An user with this username already exists."
                };

                return Task.FromResult(IdentityResult.Failed(error));
            }

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email               
            };

            // TODO: conditionally add user to LocalGuide Role 

            return userManager.CreateAsync(user, model.Password);
        }

        public async Task SignOutAsync()
        {
            await this.signInManager.SignOutAsync();
        }
    }
}
