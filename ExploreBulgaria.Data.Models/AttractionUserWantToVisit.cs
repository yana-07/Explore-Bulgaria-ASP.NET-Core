using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ExploreBulgaria.Data.Models
{
    public class AttractionUserWantToVisit
    {
        [Required]
        [ForeignKey(nameof(Attraction))]
        public string AttractionId { get; set; } = null!;

        public virtual Attraction Attraction { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;
    }
}
