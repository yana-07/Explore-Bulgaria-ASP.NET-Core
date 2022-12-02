using ExploreBulgaria.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants.Image;

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

        [ForeignKey(nameof(AddedByVisitor))]
        public string? AddedByVisitorId { get; set; }

        public virtual Visitor? AddedByVisitor { get; set; }

        [Required]
        [MaxLength(ExtensionMaxLength)]
        public string Extension { get; set; } = null!;

        public string? RemoteImageUrl { get; set; }

        [MaxLength(BlobStorageUrlMaxLength)]
        public string? BlobStorageUrl { get; set; }      
    }
}
