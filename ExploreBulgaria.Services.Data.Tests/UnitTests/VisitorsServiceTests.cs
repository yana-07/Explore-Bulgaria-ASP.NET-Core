using ExploreBulgaria.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data.Tests.UnitTests
{
    public class VisitorsServiceTests : UnitTestsBase
    {
        IVisitorsService visitorsService;

        public override void SetUp()
        {
            base.SetUp();

            visitorsService = new VisitorsService(visitorsRepo, guard);
        }

        [Test]
        public async Task CreateByUserId_ShouldWorkCorrectly()
        {
            await usersRepo.AddAsync(new ApplicationUser
            {
                UserName = "TestUserName2",
                Email = "test2@mail.com"
            });
            await usersRepo.SaveChangesAsync();

            var user = await usersRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == "test2@mail.com");

            var visitorId = await visitorsService.CreateByUserId(user!.Id);

            var dbVisitor = await visitorsRepo.GetByIdAsync(visitorId);

            Assert.IsNotNull(dbVisitor);
            Assert.That(visitorId, Is.EqualTo(dbVisitor.Id));
        }
    }
}
