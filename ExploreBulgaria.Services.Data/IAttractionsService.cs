using ExploreBulgaria.Web.ViewModels.Attractions;

namespace ExploreBulgaria.Services.Data
{
    public interface IAttractionsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(
            int page,
            AttractionsFilterModel filterModel,
            int itemsPerPage = 12);

        Task<int> GetCountAsync(AttractionsFilterModel filterModel);

        Task<T> GetByIdAsync<T>(string id);

        Task SaveTemporaryAsync(AddAttractionViewModel model, string visitorId);
    }
}
