using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Categories;
using ExploreBulgaria.Web.ViewModels.Regions;
using ExploreBulgaria.Web.ViewModels.Subcategories;
using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Services.Constants.UIConstants;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants.Attraction;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants.Region;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants.Village;
using ExploreBulgaria.Web.ViewModels.Villages;

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

        [Required(ErrorMessage = FieldIdRequired)]
        public string CategoryId { get; set; } = null!;

        public CategoryOptionViewModel? CategoryModel { get; set; }

        [StringLength(RegionNameMaxLength, MinimumLength = RegionNameMinLength, ErrorMessage = FieldLength)]
        [Display(Name = DisplayRegion)]
        public string? Region { get; set; }

        [Required(ErrorMessage = FieldIdRequired)]
        public string RegionId { get; set; } = null!;

        [StringLength(NameMaxLength, MinimumLength = VillageNameMinLength, ErrorMessage = FieldLength)]
        [Display(Name = DisplayVillage)]
        public string? Village { get; set; }

        public string? VillageId { get; set; }

        [Required(ErrorMessage = FieldIdRequired)]
        public string SubcategoryId { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        [Display(Name = DisplayLatitude)]
        public double Latitude { get; set; }

        [Required(ErrorMessage = FieldRequired)]
        [Display(Name = DisplayLongitude)]
        public double Longitude { get; set; }

        [Required(ErrorMessage = FieldIdRequired)]
        public string CreatedByVisitorId { get; set; } = null!;

        [Required]
        public string BlobNames { get; set; } = null!;

        public IEnumerable<CategoryOptionViewModel> Categories { get; set; } 
            = Enumerable.Empty<CategoryOptionViewModel>();

        public IEnumerable<SubcategoryOptionViewModel> Subcategories { get; set; } 
            = Enumerable.Empty<SubcategoryOptionViewModel>();

        public IEnumerable<RegionOptionViewModel> Regions { get; set; } 
            = Enumerable.Empty<RegionOptionViewModel>();

        public IEnumerable<VillageOptionViewModel> Villages { get; set; }
            = Enumerable.Empty<VillageOptionViewModel>();
    }
}
