using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Data.Common.Constants.DataConstants;
using static ExploreBulgaria.Data.Common.Constants.DataConstants.User;
using static ExploreBulgaria.Web.ViewModels.Common.Constants;

namespace ExploreBulgaria.Web.ViewModels.Users
{
    public class RegisterViewModel : IMapFrom<ApplicationUser>
    {
        [Required(ErrorMessage = FieldRequired)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = FieldLength)]
        [DisplayName(DisplayFirstName)]
        public string FirstName { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = FieldLength)]
        [DisplayName(DisplayLastName)]
        public string LastName { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        [StringLength(UserNameMaxLength, MinimumLength = UserNameMinLength, ErrorMessage = FieldLength)]
        [DisplayName(DisplayUserName)]
        public string UserName { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        [EmailAddress]
        [StringLength(EmailMaxLength, MinimumLength = EmailMinLength, ErrorMessage = FieldLength)]
        [DisplayName(DisplayEmail)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        [MinLength(PasswordMinLength, ErrorMessage = FieldMinLength)]
        [DisplayName(DisplayPassword)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password))]
        [DataType(DataType.Password)]
        [DisplayName(DisplayRepeatPassword)]
        public string ConfirmPassword { get; set; } = null!;
    }
}
