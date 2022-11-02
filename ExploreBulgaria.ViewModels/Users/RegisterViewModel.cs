using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Data.Common.Constants.DataConstants;
using static ExploreBulgaria.Data.Common.Constants.DataConstants.User;

namespace ExploreBulgaria.Web.ViewModels.Users
{
    public class RegisterViewModel : IMapFrom<ApplicationUser>
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [DisplayName("First Name")]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = null!;

        [Required]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength)]
        [DisplayName("Username")]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength)]
        public string Email { get; set; } = null!;

        [Required]
        [MinLength(PasswordMinLength)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
