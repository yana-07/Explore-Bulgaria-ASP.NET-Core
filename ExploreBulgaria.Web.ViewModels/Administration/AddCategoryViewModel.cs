using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Services.Constants.UIConstants;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants;

namespace ExploreBulgaria.Web.ViewModels.Administration
{
    public class AddCategoryViewModel
    {
        [Required(ErrorMessage = FieldRequired)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = FieldLength)]
        [Display(Name = DisplayName)]
        public string Name { get; set; } = null!;
    }
}
