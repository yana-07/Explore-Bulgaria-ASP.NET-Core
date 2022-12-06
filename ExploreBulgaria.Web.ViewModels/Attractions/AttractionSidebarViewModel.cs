using AutoMapper;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionSidebarViewModel : IAttractionModel, IMapFrom<Attraction>, IHaveCustomMappings
    {
        public string Id { get; set; } = null!; 

        public string Name { get; set; } = null!;

        public string? BlobStorageUrl { get; set; }

        public string? RemoteImageUrl { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? SubcategoryName { get; set; }

        public string RegionName { get; set; } = null!;

        public string? LocationName { get; set; }

        public int VisitedByVisitorsCount { get; set; }

        public int AddedToFavoritesByVisitorsCount { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Attraction, AttractionSidebarViewModel>()
                .ForMember(d => d.BlobStorageUrl, opt => opt.MapFrom(s => s.Images
                      .Where(i => i.BlobStorageUrl != null)
                      .Select(i => i.BlobStorageUrl).FirstOrDefault()))
                .ForMember(d => d.RemoteImageUrl, opt => opt.MapFrom(s => s.Images
                      .Where(i => i.RemoteImageUrl != null)
                      .Select(i => i.RemoteImageUrl)
                      .FirstOrDefault()));
        }
    }
}
