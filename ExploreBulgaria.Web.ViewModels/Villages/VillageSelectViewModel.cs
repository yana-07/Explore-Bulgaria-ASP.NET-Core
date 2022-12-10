using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Villages
{
    public class VillageSelectViewModel : IMapFrom<Village>
    {
        public string Name { get; set; } = null!;
    }
}
