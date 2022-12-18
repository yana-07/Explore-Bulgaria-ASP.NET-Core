using AutoMapper;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Data.Administration;
using ExploreBulgaria.Services.Data.Tests.Mocks;
using ExploreBulgaria.Services.Data.Tests.Models;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Administration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace ExploreBulgaria.Services.Data.Tests.UnitTests
{
    public class AdminServiceTests : UnitTestsBase
    {
        private IAdminService adminService;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            AutoMapperConfig.MapperInstance = new Mapper(new MapperConfiguration(config =>
            {
                config.CreateMap<AttractionTemporary, AttractionTempMockModel>();
            }));

            var emailSender = new EmailSenderMock();
            var logger = new Mock<ILogger<AdminService>>().Object;

            var blobClientMock = new Mock<BlobClient>();
            blobClientMock.Setup(x => x.UploadAsync(It.IsAny<Stream>()))
                .ReturnsAsync(Response.FromValue(BlobsModelFactory.BlobContentInfo(
                    It.IsAny<ETag>(), It.IsAny<DateTimeOffset>(), It.IsAny<byte[]>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<long>()), It.IsAny<Response>()));
            //var value = new Mock<Response<BlobDownloadInfo>>(MockBehavior.Loose).Object;
            //blobClientMock.Setup(x => x.DownloadAsync())
            //     .ReturnsAsync(value);

            var blobContainerClientMock = new Mock<BlobContainerClient>();
            blobContainerClientMock.Setup(x => x.GetBlobClient(It.IsAny<string>()))
                .Returns(blobClientMock.Object);

            var blobServiceClientMock = new Mock<BlobServiceClient>();
            blobServiceClientMock.Setup(x => x.GetBlobContainerClient(It.IsAny<string>()))
                .Returns(blobContainerClientMock.Object);

            adminService = new AdminService(attrTempRepo, attrRepo, categoriesRepo,
                subcategoriesRepo, regionsRepo, villagesRepo, visitorsRepo,
                blobServiceClientMock.Object, logger, emailSender, guard);
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
        [Ignore("unable to mock BlobClient.DownloadAsync")]
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
        [Ignore("unable to mock BlobClient.DownloadAsync")]
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
        public async Task GetAdminNotifications_ShouldReturnNotificationsIfThereAreSome()
        {
            await SeedDbAsync();

            var adminVisitor = await visitorsRepo
                .All()
                .FirstOrDefaultAsync();
            adminVisitor!.Notifications = "private@some-guid private@some-other-guid";
            await visitorsRepo.SaveChangesAsync();

            var notifications = await adminService.GetAdminNotifications(adminVisitor.Id);

            Assert.That(notifications.Count(), Is.EqualTo(2));
        }

        [Test]
        public async Task GetAdminNotifications_ShouldReturnEmptyEnumerableIfThereAreNoNotifications()
        {
            await SeedDbAsync();

            var adminVisitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            var notifications = await adminService.GetAdminNotifications(adminVisitor!.Id);

            Assert.True(notifications.Any() == false);
            Assert.That(notifications.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetAdminNotifications_ShouldThrowExceptionIfAdminVisitorIdNotValid()
        {
            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await adminService.GetAdminNotifications(""));
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnParticularCount()
        {
            await SeedAttractionsAsync();           

            var attractions = (await adminService.GetAllAsync<AttractionTempMockModel>(
                2, new AttractionTemporaryFilterModel(), 1)).ToList();

            Assert.That(attractions.Count, Is.EqualTo(1));
            Assert.That(attractions[0].Name, Is.EqualTo("TestAttractionTemporary2"));
        }

        [Test]
        public async Task GetAllAsync_ShouldApplyFilter()
        {
            await SeedAttractionsAsync();

            var attractions = (await adminService.GetAllAsync<AttractionTempMockModel>(
                1, new AttractionTemporaryFilterModel { SearchTerm = "2" }, 1)).ToList();

            Assert.That(attractions[0].Name, Is.EqualTo("TestAttractionTemporary2"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldWorkCorrectly()
        {
            await SeedDbAsync();

            var attraction = await adminService
                  .GetByIdAsync<AttractionTempMockModel>(1);

            Assert.That(attraction.Name, Is.EqualTo("TestAttractionTemporary1"));
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
            await SeedAttractionsAsync();

            var count = await adminService
                .GetCountAsync(new AttractionTemporaryFilterModel { SearchTerm = "2SearchTerm" });

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task NotifyAdmin_ShouldAddNotificationIfItWasNotAlreadyAdded()
        {
            await SeedDbAsync();

            var adminVisitor = await visitorsRepo
                .All()
                .Include(v => v.User)
                .FirstOrDefaultAsync();
            adminVisitor!.User.Email = "adminuser@abv.bg";
            await visitorsRepo.SaveChangesAsync();

            await adminService.NotifyAdmin("private@testGroup");

            Assert.That(adminVisitor!.Notifications, Is.EqualTo("private@testGroup"));
        }

        [Test]
        public async Task NotifyAdmin_ShouldThrowExceptionIfAdminVisitorIdNotValid()
        {
            await SeedDbAsync();

            var adminVisitor = await visitorsRepo
                .AllAsNoTracking()
                .Include(v => v.User)
                .FirstOrDefaultAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await adminService.NotifyAdmin(adminVisitor!.Id));
        }

        [Test]
        public async Task NotifyAdmin_ShouldNotAddNotificationIfItWasAlreadyAdded()
        {
            await SeedDbAsync();

            var adminVisitor = await visitorsRepo
                .All()
                .Include(v => v.User)
                .FirstOrDefaultAsync();
            adminVisitor!.User.Email = "adminuser@abv.bg";
            adminVisitor.Notifications = "private@testGroup";
            await visitorsRepo.SaveChangesAsync();

            await adminService.NotifyAdmin("private@testGroup");

            Assert.That(adminVisitor!.Notifications, Is.Not.EqualTo("private@testGroup private@testGroup"));
        }

        [Test]
        public async Task ClearAdminNotification_ShouldRemoveNotificationIfSuchExists()
        {
            await SeedDbAsync();

            var adminVisitor = await visitorsRepo
                .All()
                .Include(v => v.User)
                .FirstOrDefaultAsync();
            adminVisitor!.User.Email = "adminuser@abv.bg";
            adminVisitor.Notifications = "private@testGroup";
            await visitorsRepo.SaveChangesAsync();

            await adminService.ClearAdminNotification("private@testGroup");

            Assert.True(adminVisitor.Notifications.Contains("private @testGroup") == false);
        }

        [Test]
        public async Task ClearAdminNotification_ShouldDoNothingIfNotificationDoesNotExist()
        {
            await SeedDbAsync();

            var adminVisitor = await visitorsRepo
                .All()
                .Include(v => v.User)
                .FirstOrDefaultAsync();
            adminVisitor!.User.Email = "adminuser@abv.bg";
            adminVisitor.Notifications = "private@testGroup";
            await visitorsRepo.SaveChangesAsync();

            await adminService.ClearAdminNotification("private@someOthertestGroup");

            Assert.True(adminVisitor.Notifications.Contains("private@testGroup"));
        }

        [Test]
        public async Task ClearAdminNotification_ShouldThrowExceptionIfAdminVisitorIdNotValid()
        {
            await SeedDbAsync();

            var adminVisitor = await visitorsRepo
                .AllAsNoTracking()
                .Include(v => v.User)
                .FirstOrDefaultAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await adminService.ClearAdminNotification(adminVisitor!.Id));
        }

        [Test]
        public async Task RejectAsync_ShouldSetIsDeletedAndIsRejectedPropertyOfAttractionTemporaryToTrue()
        {
            await SeedDbAsync();

            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            await adminService.RejectAsync(1, visitor!.Id);
            var attraction = await attrTempRepo.GetByIdAsync(1);

            Assert.True(attraction!.IsRejected);
            Assert.True(attraction!.IsDeleted);
        }

        [Test]
        public async Task RejectAsync_ShouldThrowExceptionIfAttractionTemporaryIdNotValid()
        {
            await SeedDbAsync();

            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await adminService.RejectAsync(7, visitor!.Id));
        }
    }
}