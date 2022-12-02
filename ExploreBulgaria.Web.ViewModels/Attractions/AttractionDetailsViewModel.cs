using AutoMapper;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Comments;
using ExploreBulgaria.Web.ViewModels.Visitors;
using NetTopologySuite.Geometries;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionDetailsViewModel : IMapFrom<Attraction>, IHaveCustomMappings, IAttractionModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public string? SubcategoryName { get; set; }

        public string RegionName { get; set; } = null!;

        public string? LocationName { get; set; }

        public string Description { get; set; } = null!;

        public Point Coordinates { get; set; } = null!;

        public VisitorGenericViewModel AddedByVisitor { get; set; } = null!;

        public double AverageVote { get; set; }

        public IEnumerable<string> RemoteImageUrls { get; set; } 
            = Enumerable.Empty<string>();

        public IEnumerable<string> BlobStorageUrls { get; set; }
            = Enumerable.Empty<string>();

        public IEnumerable<CommentViewModel> Comments { get; set; } 
            = Enumerable.Empty<CommentViewModel>(); 

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Attraction, AttractionDetailsViewModel>()
                .ForMember(d => d.RemoteImageUrls, opt => opt.MapFrom(s => s.Images.Where(i => i.RemoteImageUrl != null).Select(i => i.RemoteImageUrl)))
                .ForMember(d => d.BlobStorageUrls, opt => opt.MapFrom(s => s.Images.Where(i => i.BlobStorageUrl != null).Select(i => i.BlobStorageUrl)))
                .ForMember(d => d.AddedByVisitor, opt => opt.MapFrom(s => s.CreatedByVisitor))
                .ForMember(d => d.AverageVote, opt => opt.MapFrom(s => s.Votes.Count == 0 ? 0 : s.Votes.Average(v => v.Value)))
                .ForMember(d => d.Comments, opt => opt.MapFrom(s => s.Comments.OrderByDescending(c => c.CreatedOn)));
        }
    }
}
