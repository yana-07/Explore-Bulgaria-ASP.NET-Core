using System.ComponentModel.DataAnnotations.Schema;

namespace ExploreBulgaria.Data.Models
{
    public class VisitorLikedComment
    {
        [ForeignKey(nameof(Visitor))]
        public string VisitorId { get; set; } = null!;

        public virtual Visitor Visitor { get; set; } = null!;

        [ForeignKey(nameof(Comment))]
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; } = null!;
    }
}
