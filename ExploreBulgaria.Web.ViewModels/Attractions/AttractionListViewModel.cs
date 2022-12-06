namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionListViewModel : BaseListViewModel
    {
        public IEnumerable<AttractionInListViewModel> Attractions { get; set; } 
            = Enumerable.Empty<AttractionInListViewModel>();
    }
}
