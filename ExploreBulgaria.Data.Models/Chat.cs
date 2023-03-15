using ExploreBulgaria.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExploreBulgaria.Data.Models
{
    public class Chat : BaseDeletableModel<int>
    {
        [Required]
        [ForeignKey(nameof(FromVisitor))]
        public string FromVisitorId { get; set; } = null!;

        public virtual Visitor FromVisitor { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(ToVisitor))]
        public string ToVisitorId { get; set; } = null!;

        public virtual Visitor ToVisitor { get; set; } = null!;

        public string Message { get; set; } = null!;

        public DateTime SentOn { get; set; }
    }
}
