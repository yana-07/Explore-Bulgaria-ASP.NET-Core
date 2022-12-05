using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExploreBulgaria.Data.Models
{
    public class VisitorVisitedAttraction
    {
        [Required]
        [ForeignKey(nameof(Visitor))]
        public string VisitorId { get; set; } = null!;

        public virtual Visitor Visitor { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Attraction))]
        public string AttractionId { get; set; } = null!;

        public virtual Attraction Attraction { get; set; } = null!;
    }
}
