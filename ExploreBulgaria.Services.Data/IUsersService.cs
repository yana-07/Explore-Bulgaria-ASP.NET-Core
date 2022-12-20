using ExploreBulgaria.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

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

    Task AddAvatarUrlClaimAsync(ApplicationUser user);

    Task AddVisitorIdClaimAsync(ApplicationUser user, string visitorId);

    Task<bool> EditProfileAsync(EditUserProfileInputModel model, string userId, string imagePath);

    Task SignOutAndInAsync(ClaimsPrincipal user);

    Task<(IdentityResult, ApplicationUser)> ExternalLoginAsync(ExternalLoginInfo extLoginInfo);
}
