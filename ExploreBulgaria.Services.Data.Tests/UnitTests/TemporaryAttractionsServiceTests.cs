using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Web.ViewModels.Attractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ExploreBulgaria.Services.Data.Tests.UnitTests
{
    public class TemporaryAttractionsServiceTests : UnitTestsBase
    {
        private ITemporaryAttractionsService attrTempService;

        public override void SetUp()
        {
            base.SetUp();

            var blobClientMock = new Mock<BlobClient>();
            blobClientMock.Setup(x => x.UploadAsync(It.IsAny<Stream>()))
                .ReturnsAsync(Response.FromValue(BlobsModelFactory.BlobContentInfo(
                    It.IsAny<ETag>(), It.IsAny<DateTimeOffset>(), It.IsAny<byte[]>(),
                    It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(),
                    It.IsAny<long>()), It.IsAny<Response>()));

            var blobContainerClientMock = new Mock<BlobContainerClient>();
            blobContainerClientMock.Setup(x => x.GetBlobClient(It.IsAny<string>()))
                .Returns(blobClientMock.Object);

            var blobServiceClientMock = new Mock<BlobServiceClient>();
            blobServiceClientMock.Setup(x => x.GetBlobContainerClient(It.IsAny<string>()))
                .Returns(blobContainerClientMock.Object);
                        
            attrTempService = new TemporaryAttractionsService(
                attrTempRepo, blobServiceClientMock.Object);
        }

        [Test]
        public async Task SaveTemporaryAsync_ShouldWorkCorrectly()
        {
            await SeedDbAsync();

            var category = await categoriesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            var model = new AddAttractionViewModel
            {
                Name = "TestAttractionTemporary2",
                CategoryId = category!.Id,
                Description = "TestDescription",
                Latitude = 27.887,
                Longitude = 27.887,
                Region = "TestRegion",
                Images = new List<IFormFile>
                {
                    new FormFile(new MemoryStream(), It.IsAny<long>(),
                    It.IsAny<long>(), It.IsAny<string>(), "test.png")
                }
            };

            await attrTempService.SaveTemporaryAsync(model, visitor!.Id);

            var tempAttractions = await attrTempRepo.AllAsNoTracking().ToListAsync();

            Assert.That(tempAttractions.Count, Is.EqualTo(2));
            Assert.That(tempAttractions[1].Name, Is.EqualTo("TestAttractionTemporary2"));
        }

        [Test]
        public async Task SaveTemporaryAsync_ShouldThrowExceptionIfImageExtensionNotSupported()
        {
            await SeedDbAsync();

            var category = await categoriesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            var model = new AddAttractionViewModel
            {
                Name = "TestAttractionTemporary2",
                CategoryId = category!.Id,
                Description = "TestDescription",
                Latitude = 27.887,
                Longitude = 27.887,
                Region = "TestRegion",
                Images = new List<IFormFile>
                {
                    new FormFile(new MemoryStream(), It.IsAny<long>(),
                    It.IsAny<long>(), It.IsAny<string>(), "test.xxx")
                }
            };

            Assert.ThrowsAsync<InvalidImageExtensionException>(
                async () => await attrTempService.SaveTemporaryAsync(model, visitor!.Id));
        }
    }
}
