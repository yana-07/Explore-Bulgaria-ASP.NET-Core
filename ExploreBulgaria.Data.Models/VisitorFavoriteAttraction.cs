using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExploreBulgaria.Data.Models
{
    public class VisitorFavoriteAttraction
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
