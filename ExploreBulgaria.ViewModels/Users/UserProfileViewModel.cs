namespace ExploreBulgaria.Web.ViewModels.Users
{
    public class UserProfileViewModel
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime JoinedOn { get; set; }

        public int AttractionsAdded { get; set; }

        public string? PhoneNumber { get; set; } 

        public string? AvatarUrl { get; set; }
    }
}
