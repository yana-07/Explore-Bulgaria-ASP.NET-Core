﻿using AutoMapper;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Web.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using static ExploreBulgaria.Data.Common.Constants.DataConstants;

namespace ExploreBulgaria.Services.Data
{
    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;

        public UsersService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
            var emailAvailable= EmailAvailable(model.Email).GetAwaiter().GetResult();
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
    }
}
