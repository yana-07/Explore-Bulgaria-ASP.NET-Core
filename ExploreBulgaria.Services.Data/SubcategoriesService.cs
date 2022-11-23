using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data
{
    public class SubcategoriesService : ISubcategoriesService
    {
        private readonly IDeletableEnityRepository<Subcategory> repo;

        public SubcategoriesService(IDeletableEnityRepository<Subcategory> repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
            => await repo.AllAsNoTracking()
                 .OrderBy(x => x.Name)
                 .To<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllForCategoryAndRegionAsync<T>(
            string categoryName,
            string? regionName = null)
        {
            var subcategories = repo.AllAsNoTracking()
                    .Where(sc => sc.Category.Name == categoryName);

            if (regionName != null)
            {
                subcategories = subcategories
                    .Where(sc => sc.Attractions.Select(a => a.Region.Name).Contains(regionName));
            }

            return await subcategories
                .OrderBy(x => x.Name)
                .To<T>().ToListAsync();
        }       
    }
}
