using ExploreBulgaria.Web.ViewModels.Categories;
using ExploreBulgaria.Web.ViewModels.Locations;
using ExploreBulgaria.Web.ViewModels.Regions;
using ExploreBulgaria.Web.ViewModels.Subcategories;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionsFilterModel : BaseFilterViewModel
    {
        public IEnumerable<CategorySelectViewModel> Categories { get; set; } 
            = Enumerable.Empty<CategorySelectViewModel>();

        public IEnumerable<SubcategorySelectViewModel> Subcategories { get; set; } 
            = Enumerable.Empty<SubcategorySelectViewModel>();

        public IEnumerable<RegionSelectViewModel> Regions { get; set; } 
            = Enumerable.Empty<RegionSelectViewModel>();

        public IEnumerable<LocationSelectViewModel> Locations { get; set; }
            = Enumerable.Empty<LocationSelectViewModel>();
    }
}
