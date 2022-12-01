namespace ExploreBulgaria.Services.Data
{
    public interface ILocationsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllForCategorySubcategoryAndRegionAsync<T>(
             string? categoryName = null,
             string? subcategoryName = null,
             string? regionName = null);
    }
}
