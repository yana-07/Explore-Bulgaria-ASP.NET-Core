using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Users
{
    public class UserGenericViewModel : IMapFrom<ApplicationUser>
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string AvatarUrl { get; set; } = null!;
    }
}
