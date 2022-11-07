namespace ExploreBulgaria.Services.Data
{
    public interface IAttractionsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(int page, int itemsPerPage = 12);

        int GetCount();
    }
}
