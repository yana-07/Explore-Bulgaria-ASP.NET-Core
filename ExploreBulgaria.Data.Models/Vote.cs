using ExploreBulgaria.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExploreBulgaria.Data.Models
{
    public class Vote: BaseDeletableModel<int>
    {
        [Required]
        [ForeignKey(nameof(Attraction))]
        public string AttractionId { get; set; } = null!;

        public virtual Attraction Attraction { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(AddedByVisitor))]
        public string AddedByVisitorId { get; set; } = null!;

        public virtual Visitor AddedByVisitor { get; set; } = null!;

        public byte Value { get; set; }     
    }
}
