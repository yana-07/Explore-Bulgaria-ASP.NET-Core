using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
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

        public async Task<IEnumerable<T>> GetAllAsync<T>(int page, int itemsPerPage = 12)
        {
            var skip = (page - 1) * itemsPerPage;

            return await repo.AllAsNoTracking()
                .To<T>()
                .Skip(skip)
                .Take(itemsPerPage)
                .ToListAsync();
        }

        public int GetCount()
            => this.repo.AllAsNoTracking().Count();
    }
}
