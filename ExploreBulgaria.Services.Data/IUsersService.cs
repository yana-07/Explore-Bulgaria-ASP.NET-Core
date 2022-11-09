using ExploreBulgaria.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace ExploreBulgaria.Web.ViewModels.Users;

public interface IUsersService
{
    Task<bool> UserNameAvailable(string userName);

    Task<bool> EmailAvailable(string email);

    (Task<IdentityResult>, ApplicationUser?) SignUpAsync(RegisterViewModel model);

    Task<SignInResult> SignInAsync(LoginViewModel model);

    Task SignOutAsync();

    Task<T> GetProfileAsync<T>(string userId);

    Task AddFirstNameClaimAsync(ApplicationUser user);

    Task AddLastNameClaimAsync(ApplicationUser user);

    Task AddEmailClaimAsync(ApplicationUser user);
}
