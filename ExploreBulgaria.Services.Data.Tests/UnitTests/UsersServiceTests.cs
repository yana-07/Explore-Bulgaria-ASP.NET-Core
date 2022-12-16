using AutoMapper;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Data.Tests.Mocks;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Services.Guards;
using ExploreBulgaria.Web.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework.Internal;
using System.Security.Claims;

namespace ExploreBulgaria.Services.Data.Tests.UnitTests
{
    public class UsersServiceTests : UnitTestsBase
    {
        private IUsersService usersService;
        private UserManager<ApplicationUser> userManager;
        public override void SetUp()
        {
            base.SetUp();

            var mapper = new Mock<IMapper>();
            mapper.Setup(m => m.Map<UserProfileViewModel>(
                It.IsAny<object>()))
                .Returns((ApplicationUser user) => new UserProfileViewModel
                {
                    FirstName = user.FirstName!,
                    LastName = user.LastName!,
                    UserName = user.UserName,
                    Email = user.Email,
                    CreatedOn = user.CreatedOn,
                    PhoneNumber = user.PhoneNumber,
                    AvatarUrl = user.AvatarUrl
                });
                
            var users = GetUsers().GetAwaiter().GetResult();
            userManager = UserManagerMock.MockUserManager(users).Object;
            usersService = new UsersService(
                userManager,
                SignInManagerMock.MockSignInManager().Object,
                usersRepo, mapper.Object, new Guard());
        }

        [Test]
        public async Task SignInAsync_ShouldSucceedForValidUser()
        {
            var model = new LoginViewModel
            {
                Email = "test-user@abv.bg",
                Password = "1234567",                
            };

            var result = await usersService.SignInAsync(model);
            Assert.True(result.Succeeded);
        }

        [Test]
        public async Task SignInAsync_ShouldFailForInvalidUser()
        {
            var model = new LoginViewModel
            {
                Email = "invalid@abv.bg",
                Password = "1234567",
            };

            var result = await usersService.SignInAsync(model);
            Assert.False(result.Succeeded);
        }

        [Test]
        public async Task SignUpAsync_ShouldSucceedIfUserNameAndEmailAreNotTaken()
        {
            var model = new RegisterViewModel
            {
                Email = "new-user@abv.bg",
                UserName = "newUser",
                Password = "1234567",
                FirstName = "New",
                LastName = "User"
            };

            (Task<IdentityResult> result, ApplicationUser? user) = usersService.SignUpAsync(model);

            Assert.True((await result).Succeeded);
            Assert.That(user!.Email, Is.EqualTo("new-user@abv.bg"));
            Assert.That(user!.UserName, Is.EqualTo("newUser"));
        }

        [Test]
        public async Task SignUpAsync_ShouldFailIfEmailIsTaken()
        {
            var model = new RegisterViewModel
            {
                Email = "test-user@abv.bg",
                UserName = "newUser",
                Password = "1234567",
                FirstName = "New",
                LastName = "User"
            };

            (Task<IdentityResult> result, ApplicationUser? user) = usersService.SignUpAsync(model);

            Assert.False((await result).Succeeded);
            Assert.IsNull(user);
        }

        [Test]
        public async Task SignUpAsync_ShouldFailIfUserNameIsTaken()
        {
            var model = new RegisterViewModel
            {
                Email = "new-user@abv.bg",
                UserName = "testUser",
                Password = "1234567",
                FirstName = "New",
                LastName = "User"
            };

            (Task<IdentityResult> result, ApplicationUser? user) = usersService.SignUpAsync(model);

            Assert.False((await result).Succeeded);
            Assert.IsNull(user);
        }

        [Test]
        public void SignOutAsync_ShouldSucceed()
        {
            Assert.DoesNotThrowAsync(async () => 
              await usersService.SignOutAsync());
        }

        [Test]
        public async Task GetProfileAsync_ShouldWorkCorrectlyForValidUserId()
        {
            var user = await usersRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == "test-user@abv.bg");

            var profile = await usersService
                .GetProfileAsync<UserProfileViewModel>(user!.Id);

            Assert.NotNull(profile);
        }

        [Test]
        public async Task EmailAvailable_ShouldReturnTrueForNonExistingEmail()
        {
            var isAvailable = await usersService.EmailAvailable("new-user@abv.bg");

            Assert.True(isAvailable);
        }

        [Test]
        public async Task EmailAvailable_ShouldReturnFalseForExistingEmail()
        {
            var isAvailable = await usersService.EmailAvailable("test-user@abv.bg");

            Assert.False(isAvailable);
        }

        [Test]
        public async Task UserNameAvailable_ShouldReturnTrueForNonExistingUserName()
        {
            var isAvailable = await usersService.UserNameAvailable("newUser");

            Assert.True(isAvailable);
        }

        [Test]
        public async Task UserNameAvailable_ShouldReturnFalseForExistingUserName()
        {
            var isAvailable = await usersService.UserNameAvailable("testUser");

            Assert.False(isAvailable);
        }

        [Test]
        public async Task AddFirstNameClaimAsync_ShouldSucceedIfFirstNameIsNotNull()
        {
            var user = await usersRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == "test-user@abv.bg");

            Assert.DoesNotThrowAsync(async () => 
               await usersService.AddFirstNameClaimAsync(user!));
        }

        [Test]
        public async Task AddFirstNameClaimAsync_ShouldThrowExceptionIfFirstNameIsNull()
        {
            var user = await usersRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == "another-test-user@abv.bg");

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
               await usersService.AddFirstNameClaimAsync(user!));
        }

        [Test]
        public async Task AddLastNameClaimAsync_ShouldSucceedIfLastNameIsNotNull()
        {
            var user = await usersRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == "test-user@abv.bg");

            Assert.DoesNotThrowAsync(async () =>
               await usersService.AddLastNameClaimAsync(user!));
        }

        [Test]
        public async Task AddLastNameClaimAsync_ShouldThrowExceptionIfLastNameIsNull()
        {
            var user = await usersRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == "another-test-user@abv.bg");

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
               await usersService.AddLastNameClaimAsync(user!));
        }

        [Test]
        public async Task AddEmailClaimAsync_ShouldSucceed()
        {
            var user = await usersRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == "test-user@abv.bg");

            Assert.DoesNotThrowAsync(async () =>
               await usersService.AddEmailClaimAsync(user!));
        }

        [Test]
        public async Task AddAvatarUrlClaimAsync_ShouldSucceedIfAvatarUrlIsNotNull()
        {
            var user = await usersRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == "test-user@abv.bg");

            Assert.DoesNotThrowAsync(async () =>
               await usersService.AddAvatarUrlClaimAsync(user!));
        }

        [Test]
        public async Task AddAvatarUrlClaimAsync_ShouldThrowExceptionIfAvatarUrlIsNull()
        {
            var user = await usersRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == "another-test-user@abv.bg");

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
               await usersService.AddAvatarUrlClaimAsync(user!));
        }

        [Test]
        public async Task AddVisitorIdClaimAsync_ShouldSucceed()
        {
            var user = await usersRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == "test-user@abv.bg");
            await visitorsRepo.AddAsync(new Visitor
            {
                UserId = user!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(v => v.UserId == user.Id);

            Assert.DoesNotThrowAsync(async () => 
               await usersService.AddVisitorIdClaimAsync(user, visitor!.Id));
        }

        [Test]
        public async Task EditProfileAsync_ShouldWorkCorrectly()
        {
            var mock = new Mock<IFormFile>();
            var model = new EditUserProfileInputModel
            {
                UserName = "newUserName",
                Email = "new-email@abv.bg",
                FirstName = "NewFirstName",
                LastName = "NewLastName",
                PhoneNumber = "0123456789",
                AvatarUrl = "new-avatar.png",
                AvatarUrlUploaded = new FormFile(new MemoryStream(),
                    It.IsAny<long>(), It.IsAny<long>(),
                    It.IsAny<string>(), "test.png")
            };

            var user = await usersRepo
                .All()
                .FirstOrDefaultAsync(u => u.Email == "test-user@abv.bg");

            bool succeeded = await usersService.EditProfileAsync(model, user!.Id, "");

            var updatedUser = await usersRepo
                .All()
                .FirstOrDefaultAsync(u => u.Email == "new-email@abv.bg");

            Assert.True(succeeded);
            //Assert.NotNull(updatedUser);
            //Assert.That(updatedUser.UserName, Is.EqualTo(model.UserName));
        }

        [Test]
        public async Task EditProfileAsync_ShouldThrowExceptionIfImageExtensionNotValid()
        {
            var model = new EditUserProfileInputModel
            {
                UserName = "newUserName",
                Email = "new-email@abv.bg",
                FirstName = "NewFirstName",
                LastName = "NewLastName",
                PhoneNumber = "0123456789",
                AvatarUrl = "new-avatar.png",
                AvatarUrlUploaded = new Mock<IFormFile>().Object
            };

            var user = await usersRepo
                .All()
                .FirstOrDefaultAsync(u => u.Email == "test-user@abv.bg");

            Assert.ThrowsAsync<InvalidImageExtensionException>(async () => 
                await usersService.EditProfileAsync(model, user!.Id, ""));
        }

        [Test]
        public void EditProfileAsync_ShouldThrowExceptionIfUserIdNotValid()
        {
            var model = new EditUserProfileInputModel
            {
                UserName = "newUserName",
                Email = "new-email@abv.bg",
                FirstName = "NewFirstName",
                LastName = "NewLastName",
                PhoneNumber = "0123456789",
                AvatarUrl = "new-avatar.png"
            };

            Assert.ThrowsAsync<ExploreBulgariaException>(async () =>
                await usersService.EditProfileAsync(model, "", ""));
        }

        [Test]
        public async Task SignOutAndInAsync_ShouldSucceed()
        {
            var user = await usersRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == "test-user@abv.bg");

            var mock = new Mock<ClaimsPrincipal>();
            
            Assert.DoesNotThrowAsync(async () => 
                await usersService.SignOutAndInAsync(mock.Object));
        }

        private async Task SeedUsersAsync()
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            var first = new ApplicationUser
            {
                Email = "test-user@abv.bg",
                UserName = "testUser",
                FirstName = "Test",
                LastName = "User",
                AvatarUrl = "testUrl.png"
            };
            first.PasswordHash = hasher.HashPassword(first, "1234567");
            await usersRepo.AddAsync(first);

            var second = new ApplicationUser
            {
                Email = "another-test-user@abv.bg",
                UserName = "another-test-user"
            };
            second.PasswordHash = hasher.HashPassword(second, "1234567");
            await usersRepo.AddAsync(second);

            var third = new ApplicationUser
            {
                Email = "some-test-user@abv.bg",
                UserName = "some-test-user"
            };
            third.PasswordHash = hasher.HashPassword(third, "1234567");
            await usersRepo.AddAsync(third);

            await usersRepo.SaveChangesAsync();
        }

        private async Task<IList<ApplicationUser>> GetUsers()
        {
            await SeedUsersAsync();

            return await usersRepo
                .AllAsNoTracking()
                .ToListAsync();
        }
    }
}
