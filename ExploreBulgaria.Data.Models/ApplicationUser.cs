using ExploreBulgaria.Data.Common.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Data.Common.Constants.DataConstants;

namespace ExploreBulgaria.Data.Models
{
    public class ApplicationUser : IdentityUser, IDeletableEntity, IAuditInfo
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();
            this.CreatedAttractions = new HashSet<Attraction>();
            this.VisitedAttractions = new HashSet<AttractionUserVisited>();
            this.WantToVisitAttractions = new HashSet<AttractionUserWantToVisit>();
            this.AddedToFavoritesAttractions = new HashSet<AttractionUserAddedToFavorites>();
        }

        [Required]
        [MaxLength(NameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(NameMaxLength)]
        public string LastName { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Attraction> CreatedAttractions { get; set; }

        public virtual ICollection<AttractionUserVisited> VisitedAttractions { get; set; }

        public virtual ICollection<AttractionUserWantToVisit> WantToVisitAttractions { get; set; }

        public virtual ICollection<AttractionUserAddedToFavorites> AddedToFavoritesAttractions { get; set; }
    }
}
