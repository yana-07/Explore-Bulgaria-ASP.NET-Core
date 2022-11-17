using AutoMapper;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Comments;
using ExploreBulgaria.Web.ViewModels.Visitors;
using ExploreBulgaria.Web.ViewModels.Votes;
using NetTopologySuite.Geometries;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionDetailsViewModel : IMapFrom<Attraction>, IHaveCustomMappings
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public string SubcategoryName { get; set; } = null!;

        public string? RegionName { get; set; }

        public string Description { get; set; } = null!;

        public Point Location { get; set; } = null!;

        public VisitorGenericViewModel AddedByVisitor { get; set; } = null!;

        public IEnumerable<string> ImageUrls { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Attraction, AttractionDetailsViewModel>()
                .ForMember(d => d.ImageUrls, opt =>
                   opt.MapFrom(s => s.Images.Select(
                       i => i.RemoteImageUrl ?? $"images/attractions/{i.Id}.{i.Extension}")))
                .ForMember(d => d.AddedByVisitor, opt => opt.MapFrom(s => s.CreatedByVisitor))
                .ForMember(d => d.Comments, opt => opt.MapFrom(s => s.Comments.OrderByDescending(c => c.CreatedOn)));
        }
    }
}
