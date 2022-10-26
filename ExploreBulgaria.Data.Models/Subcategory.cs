using ExploreBulgaria.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ExploreBulgaria.Data.Models.Constants.DataConstants;

namespace ExploreBulgaria.Data.Models
{
    public class Subcategory : BaseDeletableModel<string>
    {
        public Subcategory()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Attractions = new HashSet<Attraction>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Category))]
        public string CategoryId { get; set; } = null!;

        public virtual Category Category { get; set; } = null!;

        public virtual ICollection<Attraction> Attractions { get; set; }
    }
}
