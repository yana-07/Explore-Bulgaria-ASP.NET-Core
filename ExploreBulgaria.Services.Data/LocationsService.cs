using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data
{
    public class LocationsService : ILocationsService
    {
        private readonly IDeletableEnityRepository<Location> repo;

        public LocationsService(IDeletableEnityRepository<Location> repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
            => await repo
                .AllAsNoTracking()
                .OrderBy(l => l.Name)
                .To<T>()
                .ToListAsync();

        public async Task<IEnumerable<T>> GetAllForCategorySubcategoryAndRegionAsync<T>(string? categoryName = null, string? subcategoryName = null, string? regionName = null)
        {
            var locations = repo.AllAsNoTracking();

            if (!string.IsNullOrEmpty(categoryName))
            {
                locations = locations
                    .Where(l => l.Attractions
                       .Any(a => a.Category.Name == categoryName));
            }

            if (!string.IsNullOrEmpty(subcategoryName))
            {
                locations = locations
                    .Where(l => l.Attractions
                       .Any(a => a.Subcategory != null && 
                            a.Subcategory.Name == subcategoryName));
            }

            if (!string.IsNullOrEmpty(regionName))
            {
                locations = locations
                    .Where(l => l.Attractions
                       .Any(a => a.Region.Name == regionName));
            }

            return await locations
                .OrderBy(l => l.Name)
                .To<T>()
                .ToListAsync();
        }
    }
}
