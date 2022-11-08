namespace ExploreBulgaria.Services.Data
{
    public interface ISubcategoriesService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllForCategoryAndRegionAsync<T>(string categoryName, string? regionName = null);
    }
}
