using ExploreBulgaria.Web.ViewModels.Attractions;

namespace ExploreBulgaria.Services.Data
{
    public interface IAttractionsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(
            int page,
            string? categoryName = null,
            string? subcategoryName = null,
            string? regionName = null,
            int itemsPerPage = 12);

        Task<int> GetCountAsync(
            string? categoryName = null,
            string? subcategoryName = null,
            string? regionName = null);

        Task<T?> GetByIdAsync<T>(string id);
    }
}
