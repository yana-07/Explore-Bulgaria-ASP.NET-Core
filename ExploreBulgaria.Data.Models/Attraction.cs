using ExploreBulgaria.Data.Common.Models;
using NetTopologySuite.Geometries;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants.Attraction;

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
            this.VisitedByVisitors = new HashSet<VisitorVisitedAttraction>();
            this.AddedToFavoritesByVisitors = new HashSet<VisitorFavoriteAttraction>();
        }

        [Required]
        [MaxLength(AttractionNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Category))]
        public string CategoryId { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;

        [ForeignKey(nameof(Subcategory))]
        public string? SubcategoryId { get; set; }

        public virtual Subcategory? Subcategory { get; set; }

        [Required]
        [ForeignKey(nameof(Region))]
        public string RegionId { get; set; } = null!;

        public virtual Region Region { get; set; } = null!;

        [ForeignKey(nameof(Village))]
        public string? VillageId { get; set; }

        public virtual Village? Village { get; set; }

        [Required]
        public Point Coordinates { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(CreatedByVisitor))]
        public string CreatedByVisitorId { get; set; } = null!;

        public virtual Visitor CreatedByVisitor { get; set; } = null!;

        public virtual ICollection<Image> Images { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public virtual ICollection<VisitorVisitedAttraction> VisitedByVisitors { get; set; }

        public virtual ICollection<VisitorFavoriteAttraction> AddedToFavoritesByVisitors { get; set; }
    }
}
