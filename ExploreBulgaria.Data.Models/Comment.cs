using ExploreBulgaria.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ExploreBulgaria.Data.Common.Constants.DataConstants;

namespace ExploreBulgaria.Data.Models
{
    public class Comment : BaseDeletableModel<int>
    {
        public Comment()
        {
            this.LikedByUsers = new HashSet<UserLikedComment>();
            this.DislikedByUsers = new HashSet<UserDislikedComment>();
            this.Replies = new HashSet<Reply>();
        }

        [Required]
        [ForeignKey(nameof(Attraction))]
        public string AttractionId { get; set; } = null!;

        public virtual Attraction Attraction { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(AddedByUser))]
        public string AddedByUserId { get; set; } = null!;

        public virtual ApplicationUser AddedByUser { get; set; } = null!;

        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = null!;

        public virtual ICollection<UserLikedComment> LikedByUsers { get; set; }

        public virtual ICollection<UserDislikedComment> DislikedByUsers { get; set; }

        public virtual ICollection<Reply> Replies { get; set; }
    }
}
