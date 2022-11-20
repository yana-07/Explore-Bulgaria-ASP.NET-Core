using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Attractions;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data
{
    public class AttractionsService : IAttractionsService
    {
        private readonly IDeletableEnityRepository<Attraction> repo;

        public AttractionsService(IDeletableEnityRepository<Attraction> repo)
        {
            this.repo = repo;
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
    }
}
