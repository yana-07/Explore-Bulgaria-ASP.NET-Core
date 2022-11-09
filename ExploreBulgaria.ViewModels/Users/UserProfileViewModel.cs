using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Users
{
    public class UserProfileViewModel : IMapFrom<ApplicationUser>
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public int CreatedAttractionsCount { get; set; }

        public string? PhoneNumber { get; set; } 

        public string? AvatarUrl { get; set; }
    }
}
