using ExploreBulgaria.Web.ViewModels.Attractions;

namespace ExploreBulgaria.Services.Data
{
    public interface IAttractionsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(
            int page,
            AttractionsFilterModel filterModel,
            int itemsPerPage = 12);

        Task<int> GetCountAsync(AttractionsFilterModel filterModel);

        Task<T> GetByIdAsync<T>(string id);

        Task<IEnumerable<AttractionMineViewModel>> GetByVisitorIdAsync(
            string visitorId, int page, int itemsPerPage = 12);

        Task<int> GetCountByVisitorIdAsync(string visitorId);

        Task AddAttractionToFavorites(string visitorId, string attractionId);

        Task WantToVisitAttraction(string visitorId, string attractionId);

        Task AddAttractionToVisited(string visitorId, string attractionId);

        Task<bool> IsAddedToFavorites(string visitorId, string attractionId);

        Task<bool> IsAddedToVisited(string visitorId, string attractionId);

        Task<bool> WantToVisit(string visitorId, string attractionId);
    }
}
