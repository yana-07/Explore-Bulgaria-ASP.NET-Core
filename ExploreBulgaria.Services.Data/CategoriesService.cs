using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEnityRepository<Category> repo;

        public CategoriesService(IDeletableEnityRepository<Category> repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>()
            => await repo.AllAsNoTracking()
                 .OrderBy(x => x.Name).To<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAllForRegionAsync<T>(string regionName)
            => await repo.AllAsNoTracking()
               .Where(c => c.Attractions.Select(a => a.Region.Name)
               .Contains(regionName)).OrderBy(x => x.Name)
               .To<T>().ToListAsync();

        public async Task<T> GetByIdAsync<T>(string id)
        {
            var category = await repo.AllAsNoTracking()
                .Where(c => c.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            if (category == null)
            {
                throw new ArgumentException("Invalid Category ID.");
            }

            return category;
        }
    }
}
