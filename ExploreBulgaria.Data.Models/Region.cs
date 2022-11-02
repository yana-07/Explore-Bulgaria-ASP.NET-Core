using ExploreBulgaria.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Data.Common.Constants.DataConstants;

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
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Attraction> Attractions { get; set; }
    }
}
