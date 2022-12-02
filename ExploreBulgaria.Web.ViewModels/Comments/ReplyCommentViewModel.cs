using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Visitors;

namespace ExploreBulgaria.Web.ViewModels.Comments
{
    public class ReplyCommentViewModel : IMapFrom<Reply>
    {
        public string Text { get; set; } = null!;

        public VisitorGenericViewModel Author { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
    }
}
