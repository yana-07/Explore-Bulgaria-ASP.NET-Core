using Microsoft.AspNetCore.Identity;

namespace ExploreBulgaria.Web.ViewModels.Users;

public interface IUsersService
{
    Task<IdentityResult> SignUpAsync(RegisterViewModel model);

    Task<SignInResult> SignInAsync(LoginViewModel model);

    Task SignOutAsync();

    Task<UserProfileViewModel> GetProfileAsync(string userId);
}
