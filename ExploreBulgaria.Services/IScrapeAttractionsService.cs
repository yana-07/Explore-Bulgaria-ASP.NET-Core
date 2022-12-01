using ExploreBulgaria.Services.Models;

namespace ExploreBulgaria.Services
{
    public interface IScrapeAttractionsService
    {
        static string BaseUrl { get; } = null!;

        IEnumerable<AttractionDto> ScrapeAttractions(int start, int end);
    }
}