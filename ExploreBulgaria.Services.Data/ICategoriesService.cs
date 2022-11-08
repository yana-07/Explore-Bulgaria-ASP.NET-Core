namespace ExploreBulgaria.Services.Data
{
    public interface ICategoriesService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();

        Task<IEnumerable<T>> GetAllForRegionAsync<T>(string regionName);
    }
}
