using ExploreBulgaria.Web.ViewModels.Categories;
using NetTopologySuite.Geometries;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionByRouteViewModel
    {
        public string StartPointCoordinates { get; set; }

        public string EndPointCoordinates { get; set; }

        public IEnumerable<CategoryOptionViewModel> Categories { get; set; }
            = Enumerable.Empty<CategoryOptionViewModel>();

        public IEnumerable<string> CategoriesInput { get; set; }
            = new List<string>();
    }
}
