using AutoMapper;
using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Web.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using System.Net.NetworkInformation;
using System.Security.Claims;

namespace ExploreBulgaria.Services.Data
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IDeletableEnityRepository<ApplicationUser> repo;
        private readonly IMapper mapper;
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };

        public UsersService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IDeletableEnityRepository<ApplicationUser> repo,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.repo = repo;
            this.mapper = mapper;
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
                    Description = "Потребителското име е заето."
                };

                return (Task.FromResult(IdentityResult.Failed(error)), null);
            }

            if (!usernameAvailable)
            {
                var error = new IdentityError
                {
                    Description = "Имейлът е зает."
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

            if (user == null)
            {
                throw new ArgumentException("Invalid user ID");
            }

            return mapper.Map<T>(user);
        }

        public async Task<bool> EmailAvailable(string email)
            => await userManager.FindByEmailAsync(email) == null;

        public async Task<bool> UserNameAvailable(string userName)
            => await userManager.FindByNameAsync(userName) == null;

        public async Task AddFirstNameClaimAsync(ApplicationUser user)
            => await userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, user.FirstName));

        public async Task AddLastNameClaimAsync(ApplicationUser user)
            => await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Surname, user.LastName));

        public async Task AddEmailClaimAsync(ApplicationUser user)
            => await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Email, user.Email));

        public async Task AddAvatarUrlClaimAsync(ApplicationUser user)
            => await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Uri, user.AvatarUrl!));

        public async Task AddVisitorIdClaimAsync(ApplicationUser user, string visitorId)
            => await userManager.AddClaimAsync(user, new Claim("urn:exploreBulgaria:visitorId", visitorId));
        
        public async Task EditProfileAsync(EditUserProfileInputModel model, string userId, string imagePath)
        {
            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentNullException("Invalid userId");
            }

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
                    throw new Exception($"Invalid image extension {extension}");
                }

                var physicalPath = $"{imagePath}/avatars/{userId}.{extension}";

                using (Stream fileStream = new FileStream(physicalPath, FileMode.Create))
                {
                    await model.AvatarUrlUploaded.CopyToAsync(fileStream);
                }

                user.AvatarUrl = $"/images/avatars/{userId}.{extension}";

                await AddAvatarUrlClaimAsync(user);
            }

            await repo.SaveChangesAsync();
        }

        public async Task SignAutAndInAsync(ClaimsPrincipal user)
        {
            await signInManager.SignOutAsync();

            await signInManager.SignInAsync(
                await userManager.GetUserAsync(user), isPersistent: false);
        }
    }
}
