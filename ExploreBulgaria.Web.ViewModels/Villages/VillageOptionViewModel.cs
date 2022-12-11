using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Villages
{
    public class VillageOptionViewModel : VillageSelectViewModel, IMapFrom<Village>
    {
        public string Id { get; set; } = null!;
    }
}
