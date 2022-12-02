using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Regions
{
    public class RegionSelectViewModel : IMapFrom<Region>
    {
        public string Name { get; set; } = null!;
    }
}
