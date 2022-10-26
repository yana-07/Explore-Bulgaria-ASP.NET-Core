using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExploreBulgaria.Data.Models
{
    public class AttractionUserVisited
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
