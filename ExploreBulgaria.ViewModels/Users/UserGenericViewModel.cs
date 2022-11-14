using AutoMapper;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Users
{
    public class UserGenericViewModel : IMapFrom<ApplicationUser>,
        IMapFrom<UserLikedComment>, IMapFrom<UserDislikedComment>,
        IHaveCustomMappings
    {
        public string Id { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? AvatarUrl { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<UserLikedComment, UserGenericViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UserId));

            configuration.CreateMap<UserDislikedComment, UserGenericViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(s => s.UserId));
        }
    }
}
