using AutoMapper;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionInListViewModel : IMapFrom<Attraction>, IHaveCustomMappings, IAttractionModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public string? SubcategoryName { get; set; }

        public string RegionName { get; set; } = null!;

        public string? LocationName { get; set; }

        public string Description { get; set; } = null!;

        public string CreatedByVisitor { get; set; } = null!;

        public int CommentsCount { get; set; }

        public int ImagesCount => RemoteImageUrls.Count + BlobStorageUrls.Count;

        public DateTime CreatedOn { get; set; }

        public double AverageVote { get; set; }

        public List<string> RemoteImageUrls { get; set; } = new List<string>();

        public List<string> BlobStorageUrls { get; set; } = new List<string>();

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Attraction, AttractionInListViewModel>()
                .ForMember(d => d.RemoteImageUrls, opt => opt.MapFrom(s => s.Images.Where(img => img.RemoteImageUrl != null).Select(img => img.RemoteImageUrl)))
                .ForMember(d => d.BlobStorageUrls, opt => opt.MapFrom(s => s.Images.Where(img => img.BlobStorageUrl != null).Select(img => img.BlobStorageUrl)))
                .ForMember(d => d.Description, opt => opt.MapFrom(s => s.Description.Substring(0, 100)))
                .ForMember(d => d.AverageVote, opt => opt.MapFrom(s => s.Votes.Count == 0 ? 0 : s.Votes.Average(v => v.Value)))
                .ForMember(d => d.CreatedByVisitor, opt => opt.MapFrom(s => $"{s.CreatedByVisitor.User.FirstName} {s.CreatedByVisitor.User.LastName}"));
        }
    }
}
