using ExploreBulgaria.Web.ViewModels.Attractions;

namespace ExploreBulgaria.Services.Data
{
    public interface IAttractionsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(int page, AttractionsFilterModel? filterModel = null, int itemsPerPage = 12);

        int GetCount(AttractionsFilterModel? filterModel = null);

        Task<T?> GetByIdAsync<T>(string id);
    }
}
