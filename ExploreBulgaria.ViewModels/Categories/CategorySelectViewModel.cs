using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Categories
{
    public class CategorySelectViewModel : IMapFrom<Category>
    {
        public string Name { get; set; } = null!;
    }
}