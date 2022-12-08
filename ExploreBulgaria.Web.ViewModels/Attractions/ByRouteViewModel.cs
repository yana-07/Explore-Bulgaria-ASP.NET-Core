using ExploreBulgaria.Web.ViewModels.Categories;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class ByRouteViewModel
    {
        public string Steps { get; set; } = null!;

        public IEnumerable<CategoryOptionViewModel> Categories { get; set; }
            = Enumerable.Empty<CategoryOptionViewModel>();

        public IEnumerable<string> CategoriesInput { get; set; }
            = new List<string>();
    }
}
