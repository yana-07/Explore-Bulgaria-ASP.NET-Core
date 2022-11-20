namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionsListViewModel : PagingViewModel
    {
        public IEnumerable<AttractionInListViewModel> Attractions { get; set; } 
            = Enumerable.Empty<AttractionInListViewModel>();

        public AttractionsFilterModel FilterModel { get; set; } = null!;
    }
}
