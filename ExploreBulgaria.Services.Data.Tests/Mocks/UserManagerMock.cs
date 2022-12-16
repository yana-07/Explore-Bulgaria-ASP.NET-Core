using ExploreBulgaria.Data.Models;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;

namespace ExploreBulgaria.Services.Data.Tests.Mocks
{
    public class UserManagerMock
    {
        public static Mock<UserManager<ApplicationUser>> MockUserManager(IList<ApplicationUser> users)
        {
            var storeMock = new Mock<IUserStore<ApplicationUser>>();
            var umMock = new Mock<UserManager<ApplicationUser>>(storeMock.Object, null, null,
                null, null, null, null, null, null);
            umMock.Object.UserValidators.Add(new Mock<IUserValidator<ApplicationUser>>().Object);
            umMock.Object.PasswordValidators.Add(new Mock<IPasswordValidator<ApplicationUser>>().Object);

            umMock.Setup(um => um.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) =>
                users.FirstOrDefault(u => u.Email == email)!);

            umMock.Setup(um => um.CreateAsync(
                It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            umMock.Setup(um => um.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => users.FirstOrDefault(u => u.Id == id)!);

            umMock.Setup(um => um.FindByNameAsync(It.IsAny<string>()))
                .ReturnsAsync((string userName) => 
                   users.FirstOrDefault(u => u.UserName == userName)!);

            umMock.Setup(um => um.AddClaimAsync(
                It.IsAny<ApplicationUser>(), It.IsAny<Claim>()))
                .ReturnsAsync(IdentityResult.Success);

            umMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync((ClaimsPrincipal principal) => 
                  users.FirstOrDefault(u => u.Id == principal
                    .FindFirstValue(ClaimTypes.NameIdentifier))!);

            umMock.Setup(um => um.UpdateAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(IdentityResult.Success);

            return umMock;
        }
    }
}
