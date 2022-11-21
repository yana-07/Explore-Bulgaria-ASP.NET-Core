using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Regions
{
    public class RegionOptionViewModel : RegionSelectViewModel, IMapFrom<Region>
    {
        public string Id { get; set; } = null!;
    }
}
