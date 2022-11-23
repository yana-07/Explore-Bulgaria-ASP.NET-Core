using ExploreBulgaria.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ExploreBulgaria.Services.Common.Constants.EntityAndVMConstants;

namespace ExploreBulgaria.Data.Models
{
    public class Comment : BaseDeletableModel<int>
    {
        public Comment()
        {
            this.LikedByVisitors = new HashSet<VisitorLikedComment>();
            this.DislikedByVisitors = new HashSet<VisitorDislikedComment>();
            this.Replies = new HashSet<Reply>();
        }

        [Required]
        [ForeignKey(nameof(Attraction))]
        public string AttractionId { get; set; } = null!;

        public virtual Attraction Attraction { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(AddedByVisitor))]
        public string AddedByVisitorId { get; set; } = null!;

        public virtual Visitor AddedByVisitor { get; set; } = null!;

        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = null!;

        public virtual ICollection<VisitorLikedComment> LikedByVisitors { get; set; }

        public virtual ICollection<VisitorDislikedComment> DislikedByVisitors { get; set; }

        public virtual ICollection<Reply> Replies { get; set; }
    }
}
