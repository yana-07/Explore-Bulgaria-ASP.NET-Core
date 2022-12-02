using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Subcategories
{
    public class SubcategorySelectViewModel : IMapFrom<Subcategory>
    {
        public string Name { get; set; } = null!;
    }
}
