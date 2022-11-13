using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Users;

namespace ExploreBulgaria.Web.ViewModels.Comments
{
    public class CommentViewModel : IMapFrom<Comment>
    {
        public int Id { get; set; }

        public string AttractionId { get; set; } = null!;

        public string Text { get; set; } = null!;

        public UserGenericViewModel AddedByUser { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public IEnumerable<UserGenericViewModel> LikedByUsers { get; set; }

        public IEnumerable<UserGenericViewModel> DislikedByUsers { get; set; }
    }
}
