using AutoMapper;
using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Services.Guards;
using ExploreBulgaria.Web.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static ExploreBulgaria.Services.Constants.ExceptionConstants;
using static ExploreBulgaria.Services.Constants.MessageConstants;

namespace ExploreBulgaria.Services.Data
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IDeletableEnityRepository<ApplicationUser> repo;
        private readonly IMapper mapper;
        private readonly IGuard guard;
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };

        public UsersService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IDeletableEnityRepository<ApplicationUser> repo,
            IMapper mapper,
            IGuard guard)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.repo = repo;
            this.mapper = mapper;
            this.guard = guard;
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

        public (Task<IdentityResult>, ApplicationUser?) SignUpAsync(RegisterViewModel model)
        {
            var emailAvailable = EmailAvailable(model.Email).GetAwaiter().GetResult();
            var usernameAvailable = UserNameAvailable(model.UserName).GetAwaiter().GetResult();

            if (!emailAvailable)
            {
                var error = new IdentityError
                {
                    Description = UserNameTaken
                };

                return (Task.FromResult(IdentityResult.Failed(error)), null);
            }

            if (!usernameAvailable)
            {
                var error = new IdentityError
                {
                    Description = EmailTaken
                };

                return (Task.FromResult(IdentityResult.Failed(error)), null);
            }

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.UserName,
                Email = model.Email
            };

            // TODO: conditionally add user to LocalGuide Role 

            return (userManager.CreateAsync(user, model.Password), user);
        }

        public async Task SignOutAsync()
        {
            await this.signInManager.SignOutAsync();
        }

        public async Task<T> GetProfileAsync<T>(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            guard.AgainstNull(user, InvalidUserId);

            return mapper.Map<T>(user);
        }

        public async Task<bool> EmailAvailable(string email)
            => await userManager.FindByEmailAsync(email) == null;

        public async Task<bool> UserNameAvailable(string userName)
            => await userManager.FindByNameAsync(userName) == null;

        public async Task AddFirstNameClaimAsync(ApplicationUser user)
        {
            if (string.IsNullOrEmpty(user.FirstName))
            {
                throw new ArgumentNullException(nameof(user.FirstName));
            }

            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, user.FirstName));
        }

        public async Task AddLastNameClaimAsync(ApplicationUser user)
        {
            if (string.IsNullOrEmpty(user.LastName))
            {
                throw new ArgumentNullException(nameof(user.LastName));
            }

            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, user.LastName));
        }           

        public async Task AddEmailClaimAsync(ApplicationUser user)
            => await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));

        public async Task AddAvatarUrlClaimAsync(ApplicationUser user)
        {
            if (string.IsNullOrEmpty(user.AvatarUrl))
            {
                throw new ArgumentNullException(nameof(user.AvatarUrl));
            }

            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Uri, user.AvatarUrl));
        }           

        public async Task AddVisitorIdClaimAsync(ApplicationUser user, string visitorId)
            => await userManager.AddClaimAsync(user, new Claim("urn:exploreBulgaria:visitorId", visitorId));
        
        public async Task<bool> EditProfileAsync(EditUserProfileInputModel model, string userId, string imagePath)
        {
            var user = await userManager.FindByIdAsync(userId);

            guard.AgainstNull(user, InvalidUserId);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.PhoneNumber = model.PhoneNumber;

            if (model.AvatarUrlUploaded != null)
            {
                Directory.CreateDirectory($"{imagePath}/avatars/");
                var extension = Path.GetExtension(model.AvatarUrlUploaded.FileName)?.TrimStart('.');
                if (!allowedExtensions.Contains(extension))
                {
                    throw new InvalidImageExtensionException(
                        string.Format(InvalidImageExtension, extension)); 
                }

                var physicalPath = $"{imagePath}/avatars/{userId}.{extension}";

                using (Stream fileStream = new FileStream(physicalPath, FileMode.Create))
                {
                    await model.AvatarUrlUploaded.CopyToAsync(fileStream);
                }

                user.AvatarUrl = $"/images/avatars/{userId}.{extension}";

                await AddAvatarUrlClaimAsync(user);
            }

            try
            {
                await repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ExploreBulgariaDbException(SavingToDatabase, ex);
            }

            var result = await userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task SignOutAndInAsync(ClaimsPrincipal user)
        {
            await signInManager.SignOutAsync();

            await signInManager.SignInAsync(
                await userManager.GetUserAsync(user), isPersistent: false);
        }

        public async Task<(IdentityResult, ApplicationUser)> ExternalLoginAsync(ExternalLoginInfo extLoginInfo)
        {
            var email = extLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(email);
            IdentityResult result;
            if (user != null)
            {
                result = await userManager.AddLoginAsync(user, extLoginInfo);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                }
            }
            else
            {
                user = new ApplicationUser
                {
                    Email = email,
                    UserName = email.Substring(0, email.IndexOf('@'))
                };

                result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result =  await userManager.AddLoginAsync(user, extLoginInfo);
                }
            }

            return (result, user);
        }
    }
}
