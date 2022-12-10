using ExploreBulgaria.Web.ViewModels.Categories;
using ExploreBulgaria.Web.ViewModels.Villages;
using ExploreBulgaria.Web.ViewModels.Regions;
using ExploreBulgaria.Web.ViewModels.Subcategories;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionFilterModel : BaseFilterViewModel
    {
        public IEnumerable<CategorySelectViewModel> Categories { get; set; } 
            = Enumerable.Empty<CategorySelectViewModel>();

        public IEnumerable<SubcategorySelectViewModel> Subcategories { get; set; } 
            = Enumerable.Empty<SubcategorySelectViewModel>();

        public IEnumerable<RegionSelectViewModel> Regions { get; set; } 
            = Enumerable.Empty<RegionSelectViewModel>();

        public IEnumerable<VillageSelectViewModel> Villages { get; set; }
            = Enumerable.Empty<VillageSelectViewModel>();
    }
}
