namespace ExploreBulgaria.Web.ViewModels
{
    public abstract class BaseListViewModel : PagingViewModel
    {
        public BaseFilterViewModel FilterModel { get; set; } = null!;

        public string Area { get; set; } = null!;

        public string Controller { get; set; } = null!;

        public string Action { get; set; } = null!;
    }
}
