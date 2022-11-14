using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Users;

namespace ExploreBulgaria.Web.ViewModels.Comments
{
    public class ReplyCommentViewModel : IMapFrom<Reply>
    {
        public string Text { get; set; } = null!;

        public UserGenericViewModel Author { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
    }
}
