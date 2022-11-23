using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data
{
    public class RegionsService : IRegionsService
    {
        private readonly IDeletableEnityRepository<Region> repo;

        public RegionsService(IDeletableEnityRepository<Region> repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
            => await repo.AllAsNoTracking().
                 OrderBy(x => x.Name).To<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllForCategoryAndSubcategoryAsync<T>(string categoryName, string? subcategoryName = null)
        {
            var regions = repo.AllAsNoTracking()
                   .Where(r => r.Attractions
                     .Select(a => a.Category.Name).Contains(categoryName));

            if (subcategoryName != null)
            {
                regions = regions
                    .Where(r => r.Attractions
                       .Select(a => a.Subcategory.Name).Contains(subcategoryName));
            }

            return await regions
                .OrderBy(r => r.Name)
                .To<T>().ToListAsync();
        }
    }
}
