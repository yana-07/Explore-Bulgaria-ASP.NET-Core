using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Categories;
using ExploreBulgaria.Web.ViewModels.Regions;
using ExploreBulgaria.Web.ViewModels.Subcategories;
using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Services.Common.Constants.UIConstants;
using static ExploreBulgaria.Services.Common.Constants.EntityAndVMConstants.Attraction;
using static ExploreBulgaria.Services.Common.Constants.EntityAndVMConstants.Region;

namespace ExploreBulgaria.Web.ViewModels.Administration
{
    public class AttractionTempDetailsViewModel : IMapFrom<AttractionTemporary>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = FieldRequired)]
        [StringLength(AttractionNameMaxLength, MinimumLength = AttractionNameMinLength, ErrorMessage = FieldLength)]
        [Display(Name = DisplayName)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = FieldLength)]
        [Display(Name = DisplayDescription)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        public string CategoryId { get; set; } = null!;

        public CategoryOptionViewModel? CategoryModel { get; set; }

        [Required(ErrorMessage = FieldRequired)]
        [StringLength(RegionNameMaxLength, MinimumLength = RegionNameMinLength, ErrorMessage = FieldLength)]
        [Display(Name = DisplayRegion)]
        public string Region { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        public string RegionId { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        public string SubcategoryId { get; set; } = null!;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Required(ErrorMessage = FieldRequired)]
        public string CreatedByVisitorId { get; set; } = null!;

        [Required]
        public string BlobNames { get; set; } = null!;

        public IEnumerable<CategoryOptionViewModel> Categories { get; set; } 
            = Enumerable.Empty<CategoryOptionViewModel>();

        public IEnumerable<SubcategoryOptionViewModel> Subcategories { get; set; } 
            = Enumerable.Empty<SubcategoryOptionViewModel>();

        public IEnumerable<RegionOptionViewModel> Regions { get; set; } 
            = Enumerable.Empty<RegionOptionViewModel>();
    }
}
