using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Categories
{
    public class CategoryOptionViewModel : CategorySelectViewModel, IMapFrom<Category>
    {
        public string Id { get; set; } = null!;
    }
}
