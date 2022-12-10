using ExploreBulgaria.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants;


namespace ExploreBulgaria.Data.Models
{
    public class Village : BaseDeletableModel<string>
    {
        public Village()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Attractions = new HashSet<Attraction>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Region))]
        public string RegionId { get; set; } = null!;

        public virtual Region Region { get; set; } = null!;

        public virtual ICollection<Attraction> Attractions { get; set; }
    }
}
