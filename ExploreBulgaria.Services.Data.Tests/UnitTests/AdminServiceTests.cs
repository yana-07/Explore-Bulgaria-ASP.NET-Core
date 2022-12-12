using AutoMapper;
using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Data.Repositories;
using ExploreBulgaria.Services.Data.Administration;
using ExploreBulgaria.Services.Data.Tests.Models;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Services.Guards;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Administration;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data.Tests.UnitTests
{
    public class AdminServiceTests : UnitTestsBase
    {
        private IDeletableEnityRepository<Attraction> attrRepo;
        private IDeletableEnityRepository<AttractionTemporary> attrTempRepo;
        private IDeletableEnityRepository<Category> categoriesRepo;
        private IDeletableEnityRepository<Subcategory> subcategoriesRepo;
        private IDeletableEnityRepository<Region> regionsRepo;
        private IDeletableEnityRepository<Village> villagesRepo;
        private IDeletableEnityRepository<Visitor> visitorsRepo;
        private IAdminService adminService;
        private IGuard guard;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            attrRepo = new EfDeletableEntityRepository<Attraction>(context);
            attrTempRepo = new EfDeletableEntityRepository<AttractionTemporary>(context);
            categoriesRepo = new EfDeletableEntityRepository<Category>(context);
            subcategoriesRepo = new EfDeletableEntityRepository<Subcategory>(context);
            regionsRepo = new EfDeletableEntityRepository<Region>(context);
            villagesRepo = new EfDeletableEntityRepository<Village>(context);
            visitorsRepo = new EfDeletableEntityRepository<Visitor>(context);
            guard = new Guard();

            AutoMapperConfig.MapperInstance = new Mapper(new MapperConfiguration(config =>
            {
                config.CreateMap<AttractionTemporary, AttractionTempMockModel>();
            }));

            adminService = new AdminService(attrTempRepo, attrRepo, categoriesRepo,
                subcategoriesRepo, regionsRepo, villagesRepo, guard);
        }

        [Test]
        public async Task AddCategoryAsync_ShouldAddCategorySuccessfully()
        {
            var categoryName = "TestCategory";
            await adminService.AddCategoryAsync(categoryName);
            var addedCategory = await categoriesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == categoryName);

            Assert.That(addedCategory!.Name, Is.EqualTo(categoryName));
        }

        [Test]
        public async Task AddCategoryAsync_ShouldThrowExceptionIfCategoryAlreadyExists()
        {
            var categoryName = "TestCategory";
            await adminService.AddCategoryAsync(categoryName);

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await adminService.AddCategoryAsync(categoryName));
        }

        [Test]
        public async Task AddSubcategoryAsync_ShouldAddSubcategorySuccessfully()
        {
            await adminService.AddCategoryAsync("");
            var addedCategoryId = (await categoriesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == ""))!.Id;
            var subcategoryName = "TestSubcategory";
            await adminService.AddSubcategoryAsync(subcategoryName, addedCategoryId);
            var addedSubcategory = await subcategoriesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(sc => sc.Name == subcategoryName);

            Assert.That(addedSubcategory!.Name, Is.EqualTo(subcategoryName));
            Assert.That(addedSubcategory!.CategoryId, Is.EqualTo(addedCategoryId));
        }

        [Test]
        public async Task AddSubcategoryAsync_ShouldThrowExceptionIfSubcategoryAlreadyExists()
        {
            await adminService.AddCategoryAsync("");
            var addedCategoryId = (await categoriesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == ""))!.Id;
            var subcategoryName = "TestSubcategory";
            await adminService.AddSubcategoryAsync(subcategoryName, addedCategoryId);

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await adminService.AddSubcategoryAsync(subcategoryName, addedCategoryId));
        }

        [Test]
        public async Task AddSubcategoryAsync_ShouldThrowExceptionIfCategoryIdNotValid()
        {
            await adminService.AddCategoryAsync("");
            var addedCategoryId = (await categoriesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == ""))!.Id;
            var subcategoryName = "TestSubcategory";
            await adminService.AddSubcategoryAsync(subcategoryName, addedCategoryId);

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await adminService.AddSubcategoryAsync("2", ""));
        }

        [Test]
        public async Task AddRegionAsync_ShouldAddRegionSuccessfully()
        {
            var regionName = "TestRegion";
            await adminService.AddRegionAsync(regionName);
            var addedRegion = await regionsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == regionName);

            Assert.That(addedRegion!.Name, Is.EqualTo(regionName));
        }

        [Test]
        public async Task AddRegionAsync_ShouldThrowExceptionIfRegionAlreadyExists()
        {
            var regionName = "TestRegion";
            await adminService.AddRegionAsync(regionName);

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await adminService.AddRegionAsync(regionName));
        }

        [Test]
        public async Task AddVillageAsync_ShouldAddVillageSuccessfully()
        {
            await adminService.AddRegionAsync("");
            var addedRegionId = (await regionsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == ""))!.Id;
            var villageName = "TestVillage";
            await adminService.AddVillageAsync(villageName, addedRegionId);
            var addedVillage = await villagesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(sc => sc.Name == villageName);

            Assert.That(addedVillage!.Name, Is.EqualTo(villageName));
            Assert.That(addedVillage!.RegionId, Is.EqualTo(addedRegionId));
        }

        [Test]
        public async Task AddVillageAsync_ShouldThrowExceptionIfVillageAlreadyEXists()
        {
            await adminService.AddRegionAsync("");
            var addedRegionId = (await regionsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == ""))!.Id;
            var villageName = "TestVillage";
            await adminService.AddVillageAsync(villageName, addedRegionId);

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await adminService.AddVillageAsync(villageName, addedRegionId));
        }

        [Test]
        public async Task AddVillageAsync_ShouldThrowExceptionIfRegionIdNotValid()
        {
            await adminService.AddRegionAsync("");
            var addedRegionId = (await regionsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == ""))!.Id;
            var villageName = "TestVillage";
            await adminService.AddVillageAsync(villageName, addedRegionId);

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await adminService.AddVillageAsync("2", ""));
        }

        [Test]
        public async Task ApproveAsync_ShouldAddAttractionSuccessfully()
        {
            await SeedDbAsync();

            var attrTemp = await attrTempRepo
                  .AllAsNoTracking()
                  .FirstOrDefaultAsync()!;

            var model = new AttractionTempDetailsViewModel
            {
                Id = attrTemp!.Id,
                Name = attrTemp.Name,
                BlobNames = attrTemp.BlobNames,
                CategoryId = attrTemp.CategoryId,
                CreatedByVisitorId = attrTemp.CreatedByVisitorId,
                Description = attrTemp.Description,
                Latitude = attrTemp.Latitude,
                Longitude = attrTemp.Longitude,
                RegionId = (await regionsRepo
                     .AllAsNoTracking()
                     .FirstOrDefaultAsync(r => r.Name == "TestRegion"))!
                     .Id,
                SubcategoryId = (await subcategoriesRepo
                     .AllAsNoTracking()
                     .FirstOrDefaultAsync(r => r.Name == "TestSubcategory"))!
                     .Id,
            };

            await adminService.ApproveAsync(model);
            var attractions = await attrRepo.AllAsNoTracking().ToListAsync();

            Assert.That(attractions.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task ApproveAsync_ShouldSetIsDeletetAndIsApprovedPropertyOfAttractionTemporaryToTrue()
        {
            await SeedDbAsync();

            var attrTemp = await attrTempRepo
                  .AllAsNoTracking()
                  .FirstOrDefaultAsync()!;

            var model = new AttractionTempDetailsViewModel
            {
                Id = attrTemp!.Id,
                Name = attrTemp.Name,
                BlobNames = attrTemp.BlobNames,
                CategoryId = attrTemp.CategoryId,
                CreatedByVisitorId = attrTemp.CreatedByVisitorId,
                Description = attrTemp.Description,
                Latitude = attrTemp.Latitude,
                Longitude = attrTemp.Longitude,
                RegionId = (await regionsRepo
                     .AllAsNoTracking()
                     .FirstOrDefaultAsync(r => r.Name == "TestRegion"))!
                     .Id,
                SubcategoryId = (await subcategoriesRepo
                     .AllAsNoTracking()
                     .FirstOrDefaultAsync(r => r.Name == "TestSubcategory"))!
                     .Id,
            };

            await adminService.ApproveAsync(model);
            var attractionTemp = await attrTempRepo
                 .AllWithDeleted().FirstOrDefaultAsync();

            Assert.True(attractionTemp!.IsDeleted);
            Assert.True(attractionTemp!.IsApproved);
            Assert.False((await attrTempRepo.AllAsNoTracking().ToListAsync()).Any());
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnParticularCount()
        {
            await SeedAttractions();           

            var attractions = (await adminService.GetAllAsync<AttractionTempMockModel>(
                2, new AttractionTemporaryFilterModel(), 1)).ToList();

            Assert.That(attractions.Count, Is.EqualTo(1));
            Assert.That(attractions[0].Name, Is.EqualTo("TestAttraction2"));
        }

        [Test]
        public async Task GetAllAsync_ShouldApplyFilter()
        {
            await SeedAttractions();

            var attractions = (await adminService.GetAllAsync<AttractionTempMockModel>(
                1, new AttractionTemporaryFilterModel { SearchTerm = "2" }, 1)).ToList();

            Assert.That(attractions[0].Name, Is.EqualTo("TestAttraction2"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldWorkCorrectly()
        {
            await SeedDbAsync();

            var attraction = await adminService
                  .GetByIdAsync<AttractionTempMockModel>(1);

            Assert.That(attraction.Name, Is.EqualTo("TestAttraction"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldThrowExceptionIfAttractionIdNotValid()
        {
            await SeedDbAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await adminService.GetByIdAsync<AttractionTempMockModel>(2));
        }

        [Test]
        public async Task GetCountAsync_ShouldApplyFilterAndReturnCorrectCount()
        {
            await SeedAttractions();

            var count = await adminService
                .GetCountAsync(new AttractionTemporaryFilterModel { SearchTerm = "2SearchTerm" });

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task RejectAsync_ShouldSetIsDeletedAndIsRejectedPropertyOfAttractionTemporaryToTrue()
        {
            await SeedDbAsync();

            await adminService.RejectAsync(1);
            var attraction = await attrTempRepo.GetByIdAsync(1);

            Assert.True(attraction!.IsRejected);
            Assert.True(attraction!.IsDeleted);
        }

        [Test]
        public async Task RejectAsync_ShouldThrowExceptionIfAttractionTemporaryIdNotValid()
        {
            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await adminService.RejectAsync(1));
        }

        private async Task SeedDbAsync()
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
                Name = "TestAttraction",
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

        private async Task SeedAttractions()
        {
            await SeedDbAsync();

            await attrTempRepo.AddAsync(new AttractionTemporary
            {
                Name = "TestAttraction2",
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
        }
    }
}