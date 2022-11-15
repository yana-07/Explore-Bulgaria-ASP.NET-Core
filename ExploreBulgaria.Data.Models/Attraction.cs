using ExploreBulgaria.Data.Common.Models;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ExploreBulgaria.Data.Common.Constants.DataConstants.Attraction;

namespace ExploreBulgaria.Data.Models
{
    public class Attraction : BaseDeletableModel<string>
    {
        public Attraction()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Images = new HashSet<Image>();
            this.Comments = new HashSet<Comment>();
            this.Votes = new HashSet<Vote>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Category))]
        public string CategoryId { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Subcategory))]
        public string SubcategoryId { get; set; } = null!;

        public virtual Subcategory Subcategory { get; set; } = null!;

        public string? RegionId { get; set; }

        public virtual Region? Region { get; set; }

        [Required]
        public Point Location { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(CreatedByUser))]
        public string CreatedByUserId { get; set; } = null!;

        public virtual ApplicationUser CreatedByUser { get; set; } = null!;

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }
    }
}
