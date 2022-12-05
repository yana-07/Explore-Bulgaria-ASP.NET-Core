using ExploreBulgaria.Data.Models;

namespace ExploreBulgaria.Services.Data
{
    public interface IVisitorsService
    {
        Task<string> CreateByUserId(string userId);
    }
}
