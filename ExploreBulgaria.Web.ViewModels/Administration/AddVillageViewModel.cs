using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Services.Constants.UIConstants;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants.Village;
using ExploreBulgaria.Web.ViewModels.Regions;

namespace ExploreBulgaria.Web.ViewModels.Administration
{
    public class AddVillageViewModel
    {
        [Required(ErrorMessage = FieldRequired)]
        [StringLength(NameMaxLength, MinimumLength = VillageNameMinLength, ErrorMessage = FieldLength)]
        [Display(Name = DisplayName)]
        [RegularExpression(@"(село)\s(.*)", ErrorMessage = FieldVillageNameRegex)]
        public string Name { get; set; } = null!;

        public string RegionId { get; set; } = null!;

        public IEnumerable<RegionOptionViewModel> Regions { get; set; }
            = Enumerable.Empty<RegionOptionViewModel>();
    }
}
