using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using System.ComponentModel.DataAnnotations;
using ExploreBulgaria.Web.ViewModels.Categories;
using ExploreBulgaria.Web.ViewModels.Subcategories;
using ExploreBulgaria.Web.ViewModels.Regions;
using ExploreBulgaria.Web.ViewModels.Villages;
using static ExploreBulgaria.Services.Constants.UIConstants;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants.Attraction;
using AutoMapper;

namespace ExploreBulgaria.Web.ViewModels.Administration
{
    public class EditAttractionViewModel : IMapFrom<Attraction>, IHaveCustomMappings
    {
        public string Id { get; set; } = null!;

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

        public string? CategoryName { get; set; }

        [Required(ErrorMessage = FieldIdRequired)]
        public string SubcategoryId { get; set; } = null!;

        public string? SubcategoryName { get; set; }

        [Required(ErrorMessage = FieldIdRequired)]
        public string RegionId { get; set; } = null!;

        public string? RegionName { get; set; }

        public string? VillageId { get; set; }

        public string? VillageName { get; set; }

        [Required(ErrorMessage = FieldRequired)]
        [Display(Name = DisplayLatitude)]
        public double Latitude { get; set; }

        [Required(ErrorMessage = FieldRequired)]
        [Display(Name = DisplayLongitude)]
        public double Longitude { get; set; }

        public string? CreatedByVisitorName { get; set; }

        public IEnumerable<string> RemoteImageUrls { get; set; }
            = Enumerable.Empty<string>();

        public IEnumerable<string> BlobStorageUrls { get; set; }
            = Enumerable.Empty<string>();

        public IEnumerable<CategoryOptionViewModel> Categories { get; set; }
            = Enumerable.Empty<CategoryOptionViewModel>();

        public IEnumerable<SubcategoryOptionViewModel> Subcategories { get; set; }
            = Enumerable.Empty<SubcategoryOptionViewModel>();

        public IEnumerable<RegionOptionViewModel> Regions { get; set; }
            = Enumerable.Empty<RegionOptionViewModel>();

        public IEnumerable<VillageOptionViewModel> Villages { get; set; }
            = Enumerable.Empty<VillageOptionViewModel>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Attraction, EditAttractionViewModel>()
                .ForMember(d => d.CreatedByVisitorName, opt => opt.MapFrom(
                    s => $"{s.CreatedByVisitor.User.FirstName} {s.CreatedByVisitor.User.LastName}"))
                .ForMember(d => d.RemoteImageUrls, opt => opt.MapFrom(
                    s => s.Images.Where(i => i.RemoteImageUrl != null).Select(i => i.RemoteImageUrl)))
                .ForMember(d => d.BlobStorageUrls, opt => opt.MapFrom(
                    s => s.Images.Where(i => i.BlobStorageUrl != null).Select(i => i.BlobStorageUrl)))
                .ForMember(d => d.Latitude, opt => opt.MapFrom(s => s.Coordinates.Y))
                .ForMember(d => d.Longitude, opt => opt.MapFrom(s => s.Coordinates.X));
        }
    }
}
