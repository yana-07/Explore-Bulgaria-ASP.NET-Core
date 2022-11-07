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
            => await repo.AllAsNoTracking().To<T>().ToListAsync();
    }
}
