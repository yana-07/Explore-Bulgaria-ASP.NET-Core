using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Common.Guards;
using ExploreBulgaria.Services.Mapping;
using Microsoft.EntityFrameworkCore;
using static ExploreBulgaria.Services.Common.Constants.ExceptionConstants;

namespace ExploreBulgaria.Services.Data
{
    public class CategoriesService : ICategoriesService
    {
        private readonly IDeletableEnityRepository<Category> repo;
        private readonly IGuard guard;

        public CategoriesService(
            IDeletableEnityRepository<Category> repo,
            IGuard guard)
        {
            this.repo = repo;
            this.guard = guard;
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

            guard.AgainstNull(category, InvalidCategoryId);

            return category!;
        }
    }
}
