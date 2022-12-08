namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionByRouteListViewModel
    {
        public IEnumerable<AttractionByRouteViewModel> Attractions { get; set; }
            = Enumerable.Empty<AttractionByRouteViewModel>();
    }
}
