namespace ExploreBulgaria.Services.Data
{
    public interface ICategoriesService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();
    }
}
