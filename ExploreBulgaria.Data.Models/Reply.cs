using ExploreBulgaria.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ExploreBulgaria.Data.Common.Constants.DataConstants;

namespace ExploreBulgaria.Data.Models
{
    public class Reply : BaseDeletableModel<int>
    {
        [Required]
        [ForeignKey(nameof(Author))]
        public string AuthorId { get; set; } = null!;
        public virtual ApplicationUser Author { get; set; } = null!;

        [Required]
        [MaxLength(TextMaxLength)]
        public string Text { get; set; } = null!;

        [ForeignKey(nameof(Comment))]
        public int CommentId { get; set; }

        public virtual Comment Comment { get; set; } = null!;
    }
}
