using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Services.Constants.UIConstants;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants;
using ExploreBulgaria.Web.ViewModels.Categories;

namespace ExploreBulgaria.Web.ViewModels.Administration
{
    public class AddSubcategoryViewModel
    {

        [Required(ErrorMessage = FieldRequired)]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength, ErrorMessage = FieldLength)]
        [Display(Name = DisplayName)]
        public string Name { get; set; } = null!;

        public string CategoryId { get; set; } = null!;

        public IEnumerable<CategoryOptionViewModel> Categories { get; set; }
            = Enumerable.Empty<CategoryOptionViewModel>();   
    }
}
