namespace ExploreBulgaria.Services.Data
{
    public interface IRegionsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>();
    }
}
