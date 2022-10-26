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
        [ForeignKey(nameof(AddedByUser))]
        public string AddedByUserId { get; set; } = null!;

        public virtual ApplicationUser AddedByUser { get; set; } = null!;

        public double Value { get; set; }     
    }
}
