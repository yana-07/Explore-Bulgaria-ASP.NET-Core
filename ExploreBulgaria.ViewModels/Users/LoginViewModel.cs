using System.ComponentModel.DataAnnotations;

namespace ExploreBulgaria.Web.ViewModels.Users
{
    public class LoginViewModel
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
