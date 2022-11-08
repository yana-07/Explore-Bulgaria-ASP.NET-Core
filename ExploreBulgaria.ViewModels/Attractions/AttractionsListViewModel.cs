namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionsListViewModel : PagingViewModel
    {
        public IEnumerable<AttractionInListViewModel> Attractions { get; set; }

        public AttractionsFilterModel FilterModel { get; set; }
    }
}
