using ExploreBulgaria.Services.Models;

namespace ExploreBulgaria.Services
{
    public interface IScrapeAttractionsService
    {
        static string BaseUrl { get; }
        IEnumerable<AttractionDto> ScrapeAttractions();
    }
}