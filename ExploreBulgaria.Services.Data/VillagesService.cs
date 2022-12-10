using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data
{
    public class VillagesService : IVillagesService
    {
        private readonly IDeletableEnityRepository<Village> repo;

        public VillagesService(IDeletableEnityRepository<Village> repo)
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
            var villages = repo.AllAsNoTracking();

            if (!string.IsNullOrEmpty(categoryName))
            {
                villages = villages
                    .Where(l => l.Attractions
                       .Any(a => a.Category.Name == categoryName));
            }

            if (!string.IsNullOrEmpty(subcategoryName))
            {
                villages = villages
                    .Where(l => l.Attractions
                       .Any(a => a.Subcategory != null && 
                            a.Subcategory.Name == subcategoryName));
            }

            if (!string.IsNullOrEmpty(regionName))
            {
                villages = villages
                    .Where(l => l.Attractions
                       .Any(a => a.Region.Name == regionName));
            }

            return await villages
                .OrderBy(l => l.Name)
                .To<T>()
                .ToListAsync();
        }
    }
}
