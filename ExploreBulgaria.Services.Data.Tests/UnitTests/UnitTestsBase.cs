using ExploreBulgaria.Data;
using ExploreBulgaria.Services.Data.Tests.Mocks;
using Microsoft.Extensions.DependencyInjection;

namespace ExploreBulgaria.Services.Data.Tests.UnitTests
{
    [TestFixture]
    public class UnitTestsBase
    {
        protected ApplicationDbContext context;

        protected IServiceCollection serviceCollection;

        [SetUp]
        public virtual void SetUp()
        {
            context = new DatabaseMock().CreateContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }
    }
}
