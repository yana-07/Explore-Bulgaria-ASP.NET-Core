using ExploreBulgaria.Web.ViewModels.Administration;

namespace ExploreBulgaria.Services.Data
{
    public interface ITemporaryAttractionsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(
            int page,
            AttractionTemporaryFilterModel filterModel,
            int itemsPerPage);

        Task<int> GetCountAsync(AttractionTemporaryFilterModel filterModel);
    }
}
