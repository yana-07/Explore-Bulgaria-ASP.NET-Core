namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class SidebarComponentViewModel
    {
        public IEnumerable<AttractionSidebarViewModel> MostFavorite { get; set; }
             = Enumerable.Empty<AttractionSidebarViewModel>();

        public IEnumerable<AttractionSidebarViewModel> MostVisited { get; set; }
             = Enumerable.Empty<AttractionSidebarViewModel>();

        public IEnumerable<AttractionSidebarViewModel> Newest { get; set; }
             = Enumerable.Empty<AttractionSidebarViewModel>();
    }
}
