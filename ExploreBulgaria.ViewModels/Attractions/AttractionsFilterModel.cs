using ExploreBulgaria.Web.ViewModels.Categories;
using ExploreBulgaria.Web.ViewModels.Regions;
using ExploreBulgaria.Web.ViewModels.Subcategories;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionsFilterModel
    {
        public string? CategoryName { get; set; }

        public string? SubcategoryName { get; set; }

        public string? RegionName { get; set; }

        public string? SearchTerm { get; set; }

        public IEnumerable<CategorySelectViewModel> Categories { get; set; } 
            = Enumerable.Empty<CategorySelectViewModel>();

        public IEnumerable<SubcategorySelectViewModel> Subcategories { get; set; } 
            = Enumerable.Empty<SubcategorySelectViewModel>();

        public IEnumerable<RegionSelectViewModel> Regions { get; set; } 
            = Enumerable.Empty<RegionSelectViewModel>();
    }
}
