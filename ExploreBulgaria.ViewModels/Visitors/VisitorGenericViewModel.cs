using AutoMapper;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Visitors
{
    public class VisitorGenericViewModel : IMapFrom<Visitor>,
        IMapFrom<VisitorLikedComment>, IMapFrom<VisitorDislikedComment>, IHaveCustomMappings
    {
        public string Id { get; set; } = null!;

        public string UserId { get; set; } = null!;

        public string? UserFirstName { get; set; }

        public string? UserLastName { get; set; }

        public string? UserAvatarUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<VisitorLikedComment, VisitorGenericViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.VisitorId));

            configuration.CreateMap<VisitorDislikedComment, VisitorGenericViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.VisitorId));
        }
    }
}
