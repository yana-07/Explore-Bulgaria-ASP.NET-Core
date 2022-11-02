using ExploreBulgaria.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ExploreBulgaria.Data.Common.Constants.DataConstants.Image;

namespace ExploreBulgaria.Data.Models
{
    public class Image : BaseDeletableModel<string>
    {
        public Image()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        [Required]
        [ForeignKey(nameof(Attraction))]
        public string AttractionId { get; set; } = null!;

        public virtual Attraction Attraction { get; set; } = null!;

        [ForeignKey(nameof(AddedByUser))]
        public string? AddedByUserId { get; set; }

        public virtual ApplicationUser? AddedByUser { get; set; }

        [Required]
        [MaxLength(ExtensionMaxLength)]
        public string Extension { get; set; } = null!;

        public string? RemoteImageUrl { get; set; }      
    }
}
