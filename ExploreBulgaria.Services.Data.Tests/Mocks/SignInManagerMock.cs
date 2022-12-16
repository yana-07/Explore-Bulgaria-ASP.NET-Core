using ExploreBulgaria.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ExploreBulgaria.Services.Data.Tests.Mocks
{
    public class SignInManagerMock
    {
        public static Mock<SignInManager<ApplicationUser>> MockSignInManager()
        {
            var umMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);
            var contextAccessorMock = new Mock<IHttpContextAccessor>();
            var userPrincipalFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
            var simMock = new Mock<SignInManager<ApplicationUser>>(
                umMock.Object, contextAccessorMock.Object,
                userPrincipalFactoryMock.Object, null, null, null);

            simMock.Setup(sim => sim.PasswordSignInAsync(It.IsAny<ApplicationUser>(),
                It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);

            simMock.Setup(sim => sim.SignInAsync(It.IsAny<ApplicationUser>(),
                It.IsAny<bool>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            simMock.Setup(sim => sim.SignOutAsync())
                .Returns(Task.CompletedTask);

            return simMock;
        }
    }
}
