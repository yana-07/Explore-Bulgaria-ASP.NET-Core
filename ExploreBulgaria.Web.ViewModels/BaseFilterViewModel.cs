namespace ExploreBulgaria.Web.ViewModels
{
    public abstract class BaseFilterViewModel
    {
        public string? CategoryName { get; set; }

        public string? SubcategoryName { get; set; }

        public string? RegionName { get; set; }

        public string? LocationName { get; set; }

        public string? SearchTerm { get; set; }
    }
}
