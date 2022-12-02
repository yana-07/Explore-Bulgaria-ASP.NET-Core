namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionMineListViewModel : BaseListViewModel
    {
        public IEnumerable<AttractionMineViewModel> Attractions { get; set; } 
            = Enumerable.Empty<AttractionMineViewModel>();
    }
}
