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
            => await repo.AllAsNoTracking().To<T>().ToListAsync();
    }
}
