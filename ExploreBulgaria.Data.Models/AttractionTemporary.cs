using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Data.Common.Constants.DataConstants.Attraction;
using NetTopologySuite.Geometries;
using ExploreBulgaria.Data.Common.Models;

namespace ExploreBulgaria.Data.Models
{
    public class AttractionTemporary : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string CategoryId { get; set; } = null!;

        [Required]
        public Point Location { get; set; } = null!;

        [Required]
        [MaxLength(NameMaxLength)]
        public string Region { get; set; } = null!;

        [Required]
        public string CreatedByVisitorId { get; set; } = null!;
    }
}
