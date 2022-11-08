using AutoMapper;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionInListViewModel : IMapFrom<Attraction>, IHaveCustomMappings
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? RegionName { get; set; }

        public string Description { get; set; } = null!;

        public string CreatedByUser { get; set; } = null!;

        public int CommentsCount { get; set; }

        public int ImagesCount => ImageUrls.Count;

        public DateTime CreatedOn { get; set; }

        public List<string> ImageUrls { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Attraction, AttractionInListViewModel>()
                .ForMember(d => d.ImageUrls, opt => opt.MapFrom(s => s.Images.Select(img => img.RemoteImageUrl)))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description.Substring(0, 100)))
                .ForMember(d => d.CreatedByUser, opt => opt.MapFrom(s => $"{s.CreatedByUser.FirstName} {s.CreatedByUser.LastName}"));
        }
    }
}
