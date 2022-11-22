using ExploreBulgaria.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Data.Common.Constants.DataConstants.Region;

namespace ExploreBulgaria.Data.Models
{
    public class Region : BaseDeletableModel<string>
    {
        public Region()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Attractions = new HashSet<Attraction>();
        }

        [Required]
        [MaxLength(RegionNameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Attraction> Attractions { get; set; }
    }
}
