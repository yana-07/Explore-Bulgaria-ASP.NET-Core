using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants.Region;
using static ExploreBulgaria.Services.Constants.UIConstants;

namespace ExploreBulgaria.Web.ViewModels.Administration
{
    public class AddRegionViewModel
    {
        [Required(ErrorMessage = FieldRequired)]
        [StringLength(RegionNameMaxLength, MinimumLength = RegionNameMinLength, ErrorMessage = FieldLength)]
        [Display(Name = DisplayName)]
        public string Name { get; set; } = null!;
    }
}
