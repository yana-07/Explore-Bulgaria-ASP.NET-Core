namespace ExploreBulgaria.Services.Data
{
    public interface IRegionsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllForCategoryAndSubcategoryAsync<T>(string categoryName, string? subcategoryName = null);
    }
}
