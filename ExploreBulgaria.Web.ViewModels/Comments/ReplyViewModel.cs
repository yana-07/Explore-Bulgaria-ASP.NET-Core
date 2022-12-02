using ExploreBulgaria.Web.ViewModels.Visitors;

namespace ExploreBulgaria.Web.ViewModels.Comments
{
    public class ReplyViewModel
    {
        public string Text { get; set; } = null!;

        public VisitorGenericViewModel AddedByVisitor { get; set; } = null!;

        public int RepliesCount { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
