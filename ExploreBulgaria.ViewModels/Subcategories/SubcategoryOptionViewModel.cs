using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Subcategories
{
    public class SubcategoryOptionViewModel : SubcategorySelectViewModel, IMapFrom<Subcategory>
    {
        public string Id { get; set; } = null!;
    }
}
