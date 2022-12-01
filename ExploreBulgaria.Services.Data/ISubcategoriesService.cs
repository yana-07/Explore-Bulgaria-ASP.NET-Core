namespace ExploreBulgaria.Services.Data
{
    public interface ISubcategoriesService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllForCategory<T>(string categoryName);
    }
}
