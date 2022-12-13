using ExploreBulgaria.Data;
using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Data.Repositories;
using ExploreBulgaria.Services.Data.Tests.Mocks;
using ExploreBulgaria.Services.Guards;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExploreBulgaria.Services.Data.Tests.UnitTests
{
    [TestFixture]
    public class UnitTestsBase
    {
        protected ApplicationDbContext context;

        protected IDeletableEnityRepository<Attraction> attrRepo;
        protected IDeletableEnityRepository<AttractionTemporary> attrTempRepo;
        protected IDeletableEnityRepository<Category> categoriesRepo;
        protected IDeletableEnityRepository<Subcategory> subcategoriesRepo;
        protected IDeletableEnityRepository<Region> regionsRepo;
        protected IDeletableEnityRepository<Village> villagesRepo;
        protected IDeletableEnityRepository<Visitor> visitorsRepo;
        protected IGuard guard;

        [SetUp]
        public virtual void SetUp()
        {
            context = new DatabaseMock().CreateContext();
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            attrRepo = new EfDeletableEntityRepository<Attraction>(context);
            attrTempRepo = new EfDeletableEntityRepository<AttractionTemporary>(context);
            categoriesRepo = new EfDeletableEntityRepository<Category>(context);
            subcategoriesRepo = new EfDeletableEntityRepository<Subcategory>(context);
            regionsRepo = new EfDeletableEntityRepository<Region>(context);
            villagesRepo = new EfDeletableEntityRepository<Village>(context);
            visitorsRepo = new EfDeletableEntityRepository<Visitor>(context);
            guard = new Guard();
        }

        [TearDown]
        public void TearDown()
        {
            context.Dispose();
        }

        protected async Task SeedDbAsync()
        {
            await categoriesRepo.AddAsync(new Category
            {
                Name = "TestCategory"
            });
            await categoriesRepo.SaveChangesAsync();

            await subcategoriesRepo.AddAsync(new Subcategory
            {
                Name = "TestSubcategory",
                CategoryId = (await categoriesRepo
                   .AllAsNoTracking()
                   .FirstOrDefaultAsync(c => c.Name == "TestCategory"))!
                   .Id
            });
            await subcategoriesRepo.SaveChangesAsync();

            await visitorsRepo.AddAsync(new Visitor
            {
                User = new ApplicationUser
                {
                    UserName = "TestUserName",
                    Email = "test@mail.com"
                }
            });
            await visitorsRepo.SaveChangesAsync();

            await regionsRepo.AddAsync(new Region
            {
                Name = "TestRegion"
            });
            await regionsRepo.SaveChangesAsync();

            await attrTempRepo.AddAsync(new AttractionTemporary
            {
                Name = "TestAttractionTemporary1",
                CategoryId = (await categoriesRepo
                    .AllAsNoTracking()
                    .FirstOrDefaultAsync(c => c.Name == "TestCategory"))!.Id,
                BlobNames = "test-blob-names",
                Description = "TestDescription",
                Latitude = 12.345,
                Longitude = 12.345,
                CreatedByVisitorId = (await visitorsRepo
                    .AllAsNoTracking()
                    .Include(v => v.User)
                    .FirstOrDefaultAsync(v => v.User.UserName == "TestUserName"))!.Id,
                Region = "TestRegion"
            });
            await attrTempRepo.SaveChangesAsync();
        }

        protected async Task SeedAttractionsAsync()
        {
            await SeedDbAsync();

            await attrTempRepo.AddAsync(new AttractionTemporary
            {
                Name = "TestAttractionTemporary2",
                CategoryId = (await categoriesRepo
                    .AllAsNoTracking()
                    .FirstOrDefaultAsync(c => c.Name == "TestCategory"))!.Id,
                BlobNames = "test-blob-names",
                Description = "TestDescription2SearchTerm",
                Latitude = 12.345,
                Longitude = 12.345,
                CreatedByVisitorId = (await visitorsRepo
                    .AllAsNoTracking()
                    .Include(v => v.User)
                    .FirstOrDefaultAsync(v => v.User.UserName == "TestUserName"))!.Id,
                Region = "TestRegion"
            });
            await attrTempRepo.SaveChangesAsync();

            await attrRepo.AddAsync(new Attraction
            {
                Name = "TestAttraction1",
                CategoryId = (await categoriesRepo
                    .AllAsNoTracking()
                    .FirstOrDefaultAsync(c => c.Name == "TestCategory"))!.Id,
                SubcategoryId = (await subcategoriesRepo
                    .AllAsNoTracking()
                    .FirstOrDefaultAsync())!.Id,           
                Description = "TestDescription1",
                CreatedByVisitorId = (await visitorsRepo
                    .AllAsNoTracking()
                    .Include(v => v.User)
                    .FirstOrDefaultAsync(v => v.User.UserName == "TestUserName"))!.Id,
                RegionId = (await regionsRepo
                    .AllAsNoTracking()
                    .FirstOrDefaultAsync())!.Id
            });
            await attrRepo.SaveChangesAsync();
        }
    }
}
