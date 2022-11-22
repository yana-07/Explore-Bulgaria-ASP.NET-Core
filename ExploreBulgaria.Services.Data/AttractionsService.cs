using Azure.Storage.Blobs;
using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Attractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ExploreBulgaria.Services.Data
{
    public class AttractionsService : IAttractionsService
    {
        private readonly string[] allowedExtensions = new[] { "jpg", "png", "gif" };

        private readonly IDeletableEnityRepository<Attraction> repo;
        private readonly IDeletableEnityRepository<AttractionTemporary> repoTemp;
        private readonly BlobServiceClient blobServiceClient;

        public AttractionsService(
            IDeletableEnityRepository<Attraction> repo,
            IDeletableEnityRepository<AttractionTemporary> repoTemp,
            BlobServiceClient blobServiceClient)
        {
            this.repo = repo;
            this.repoTemp = repoTemp;
            this.blobServiceClient = blobServiceClient;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(
            int page,
            AttractionsFilterModel filterModel,
            int itemsPerPage = 12)
        {
            var skip = (page - 1) * itemsPerPage;

            var attractions = ApplyFilter(
                filterModel.CategoryName, filterModel.SubcategoryName,
                filterModel.RegionName, filterModel.SearchTerm);

            return await attractions
                .To<T>()
                .Skip(skip)
                .Take(itemsPerPage)
                .ToListAsync();          
        }

        public async Task<int> GetCountAsync(AttractionsFilterModel filterModel)
        {
            var attractions = ApplyFilter(
                filterModel.CategoryName, filterModel.SubcategoryName,
                filterModel.RegionName, filterModel.SearchTerm);

            return await attractions.CountAsync();
        }

        public async Task<T?> GetByIdAsync<T>(string id)
            => await repo.AllAsNoTracking()
                 .Where(a => a.Id == id)
                 .To<T>()
                 .FirstOrDefaultAsync();

        private IQueryable<Attraction> ApplyFilter(
            string? categoryName = null,
            string? subcategoryName = null,
            string? regionName = null,
            string? searchTerm = null)
        {
            var result = repo
                .AllAsNoTracking();

            if (string.IsNullOrEmpty(categoryName) == false)
            {
                result = result
                    .Where(a => a.Category.Name == categoryName);
            }

            if (string.IsNullOrEmpty(subcategoryName) == false)
            {
                result = result
                    .Where(a => a.Subcategory.Name == subcategoryName);
            }

            if (string.IsNullOrEmpty(regionName) == false)
            {
                result = result
                    .Where(a => a.Region != null && a.Region.Name == regionName);
            }

            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                result = result
                    .Where(a => EF.Functions.Like(a.Name.ToLower(), searchTerm) ||
                        EF.Functions.Like(a.Description.ToLower(), searchTerm));
            }

            return result;
        }

        public async Task SaveTemporaryAsync(AddAttractionViewModel model, string visitorId)
        {
            var attractionTemp = new AttractionTemporary
            {
                Name = model.Name,
                Description = model.Description,
                Region = model.Region,
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
           
           await repoTemp.AddAsync(attractionTemp);

           await repoTemp.SaveChangesAsync();
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
