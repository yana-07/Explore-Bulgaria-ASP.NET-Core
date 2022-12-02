using ExploreBulgaria.Web.ViewModels.Attractions;

namespace ExploreBulgaria.Web.ViewModels.Administration
{
    public class AttractionTemporaryListViewModel : BaseListViewModel
    {
        public IEnumerable<AttractionTemporaryViewModel> Attractions { get; set; } 
            = Enumerable.Empty<AttractionTemporaryViewModel>();
    }
}
