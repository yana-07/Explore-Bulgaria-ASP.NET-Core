namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionsListViewModel : BaseListViewModel
    {
        public IEnumerable<AttractionInListViewModel> Attractions { get; set; } 
            = Enumerable.Empty<AttractionInListViewModel>();
    }
}
