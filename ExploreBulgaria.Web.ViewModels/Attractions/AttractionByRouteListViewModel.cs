namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionByRouteListViewModel
    {
        public string Steps { get; set; } = null!;

        public int Page { get; set; }

        public IEnumerable<AttractionByRouteViewModel> Attractions { get; set; }
            = Enumerable.Empty<AttractionByRouteViewModel>();

        public int PagesCount { get; set; }

        public IEnumerable<string> CategoriesInput { get; set; }
            = new List<string>();
    }
}
