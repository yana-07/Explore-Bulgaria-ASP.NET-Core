using Azure.Storage.Blobs;
using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Web.ViewModels.Attractions;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace ExploreBulgaria.Services.Data
{
    public class TemporaryAttractionsService : ITemporaryAttractionsService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };

        private readonly IDeletableEnityRepository<AttractionTemporary> repo;
        private readonly BlobServiceClient blobServiceClient;

        public TemporaryAttractionsService(
            IDeletableEnityRepository<AttractionTemporary> repo,
            BlobServiceClient blobServiceClient)
        {
            this.repo = repo;
            this.blobServiceClient = blobServiceClient;
        }    

        public async Task SaveTemporaryAsync(AddAttractionViewModel model, string visitorId)
        {
            var attractionTemp = new AttractionTemporary
            {
                Name = model.Name,
                Description = model.Description,
                Region = model.Region,
                Village = model.Village,
                CategoryId = model.CategoryId,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                CreatedByVisitorId = visitorId
            };

            var sb = new StringBuilder();

            foreach (var image in model.Images)
            {
                var extension = Path.GetExtension(image.FileName).TrimStart('.');
                if (!allowedExtensions.Any(x => extension.EndsWith(x)))
                {
                    throw new Exception($"Invalid image extension {extension}");
                }

                var blobName = $"{Guid.NewGuid()}.{extension}";
                sb.Append($"{blobName}, ");

                await UploadImageAsync(image, blobName);
            }

            attractionTemp.BlobNames = sb.ToString().Trim();

            await repo.AddAsync(attractionTemp);

            await repo.SaveChangesAsync();
        }

        private async Task UploadImageAsync(IFormFile image, string blobName)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient("attractions");
            var blobClient = containerClient.GetBlobClient(blobName);

            using (var stream = image.OpenReadStream())
            {
                await blobClient.UploadAsync(stream);
            }
        }        
    }
}
