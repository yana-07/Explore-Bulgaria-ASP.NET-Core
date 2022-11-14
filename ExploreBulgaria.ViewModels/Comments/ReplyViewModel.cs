using ExploreBulgaria.Web.ViewModels.Users;

namespace ExploreBulgaria.Web.ViewModels.Comments
{
    public class ReplyViewModel
    {
        public string Text { get; set; } = null!;

        public UserGenericViewModel AddedByUser { get; set; } = null!;

        public int RepliesCount { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
