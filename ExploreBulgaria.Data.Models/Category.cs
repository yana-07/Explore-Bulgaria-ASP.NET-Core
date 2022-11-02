using ExploreBulgaria.Data.Common.Models;
using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Data.Common.Constants.DataConstants;

namespace ExploreBulgaria.Data.Models
{
    public class Category : BaseDeletableModel<string>
    {
        public Category()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Subcategories = new HashSet<Subcategory>();
            this.Attractions = new HashSet<Attraction>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Subcategory> Subcategories { get; set; }

        public virtual ICollection<Attraction> Attractions { get; set; }
    }
}
