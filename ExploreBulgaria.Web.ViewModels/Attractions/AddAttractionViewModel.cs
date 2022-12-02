using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Categories;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Services.Constants.UIConstants;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants.Attraction;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants.Region;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AddAttractionViewModel 
    {
        [Required(ErrorMessage = FieldRequired)]
        [StringLength(AttractionNameMaxLength, MinimumLength = AttractionNameMinLength, ErrorMessage = FieldLength)]
        [Display(Name = DisplayName)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = FieldLength)]
        [Display(Name = DisplayDescription)]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        [StringLength(RegionNameMaxLength,MinimumLength = RegionNameMinLength, ErrorMessage = FieldLength)]
        [Display(Name = DisplayRegion)]
        public string Region { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        public string CategoryId { get; set; } = null!;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public IEnumerable<CategoryOptionViewModel> Categories { get; set; } 
            = Enumerable.Empty<CategoryOptionViewModel>();

        public IEnumerable<IFormFile> Images { get; set; } 
            = Enumerable.Empty<IFormFile>();
    }
}
