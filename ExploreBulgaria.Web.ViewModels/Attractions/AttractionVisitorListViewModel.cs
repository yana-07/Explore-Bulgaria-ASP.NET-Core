namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionVisitorListViewModel : BaseListViewModel
    {
        public IEnumerable<AttractionSimpleViewModel> Attractions { get; set; } 
            = Enumerable.Empty<AttractionSimpleViewModel>();       
    }
}
