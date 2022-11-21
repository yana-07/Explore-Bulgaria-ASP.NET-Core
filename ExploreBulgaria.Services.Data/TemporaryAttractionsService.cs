using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Administration;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data
{
    public class TemporaryAttractionsService : ITemporaryAttractionsService
    {
        private readonly IDeletableEnityRepository<AttractionTemporary> repo;

        public TemporaryAttractionsService(
            IDeletableEnityRepository<AttractionTemporary> repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(int page,
            AttractionTemporaryFilterModel filterModel,
            int itemsPerPage)
        {
            var skip = (page - 1) * itemsPerPage;

            var attractionsTemp = ApplyFilter(filterModel);

            return await attractionsTemp
                .Skip(skip)
                .Take(itemsPerPage)
                .To<T>()
                .ToListAsync();
        }

        public async Task<int> GetCountAsync(AttractionTemporaryFilterModel filterModel)
        {
            var attractionsTemp = ApplyFilter(filterModel);

            return await attractionsTemp.CountAsync();
        }

        private IQueryable<AttractionTemporary> ApplyFilter(AttractionTemporaryFilterModel filterModel)
        {
            var result = repo.AllAsNoTracking();

            var searchTerm = filterModel.SearchTerm;

            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                result = result.Where(at => EF.Functions.Like(at.Name.ToLower(), searchTerm) ||
                        EF.Functions.Like(at.Description.ToLower(), searchTerm));
            }

            return result;
        }
    }
}
