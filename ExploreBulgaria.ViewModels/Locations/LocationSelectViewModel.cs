using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Locations
{
    public class LocationSelectViewModel : IMapFrom<Location>
    {
        public string Name { get; set; } = null!;
    }
}
