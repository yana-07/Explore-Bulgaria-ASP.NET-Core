using ExploreBulgaria.Data.Models;

namespace ExploreBulgaria.Services.Data
{
    public interface IVisitorsService
    {
        Task<string> CreateByUserId(string userId);

        Task AddAttractionToFavorites(string visitorId, string attractionId);

        Task WantToVisitAttraction(string visitorId, string attractionId);

        Task AddAttractionToVisited(string visitorId, string attractionId);

        Task<bool> IsAddedToFavorites(string visitorId, string attractionId);

        Task<bool> IsAddedToVisited(string visitorId, string attractionId);

        Task<bool> WantToVisit(string visitorId, string attractionId);
    }
}
