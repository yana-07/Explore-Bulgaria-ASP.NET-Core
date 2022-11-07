namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionsListViewModel : PagingViewModel
    {
        public IEnumerable<AttractionInListViewModel> Attractions { get; set; }

        public IEnumerable<CategorySelectViewModel> Categories { get; set; }

        public IEnumerable<SubcategorySelectViewModel> Subcategories { get; set; }

        public IEnumerable<RegionSelectViewModel> Regions { get; set; }
    }
}
