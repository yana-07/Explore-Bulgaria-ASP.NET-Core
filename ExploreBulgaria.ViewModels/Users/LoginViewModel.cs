using Microsoft.AspNetCore.Authentication;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Web.ViewModels.Common.Constants;

namespace ExploreBulgaria.Web.ViewModels.Users
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = FieldRequired)]
        [DisplayName(DisplayEmail)]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        [DisplayName(DisplayPassword)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public IList<AuthenticationScheme> ExternalLogins { get; set; } 
            = new List<AuthenticationScheme>();
    }
}
