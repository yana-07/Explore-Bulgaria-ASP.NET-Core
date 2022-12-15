using ExploreBulgaria.Data.Common.Models;
using System.ComponentModel.DataAnnotations;

namespace ExploreBulgaria.Data.Models
{
    public class Visitor : BaseDeletableModel<string>
    {
        public Visitor()
        {
            this.Id = Guid.NewGuid().ToString();
            this.CreatedAttractions = new HashSet<Attraction>();
            this.LikedComments = new HashSet<VisitorLikedComment>();
            this.DislikedComments = new HashSet<VisitorDislikedComment>();
            this.FavoriteAttractions = new HashSet<VisitorFavoriteAttraction>();
            this.WantToVisitAttractions = new HashSet<VisitorWantToVisitAttraction>();
            this.VisitedAttractions = new HashSet<VisitorVisitedAttraction>();
        }

        [Required]
        public string UserId { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;

        public string? Notifications { get; set; }

        public virtual ICollection<Attraction> CreatedAttractions { get; set; }

        public virtual ICollection<VisitorLikedComment> LikedComments { get; set; }

        public virtual ICollection<VisitorDislikedComment> DislikedComments { get; set; }

        public virtual ICollection<VisitorFavoriteAttraction> FavoriteAttractions { get; set; }

        public virtual ICollection<VisitorWantToVisitAttraction> WantToVisitAttractions { get; set; }

        public virtual ICollection<VisitorVisitedAttraction> VisitedAttractions { get; set; }
    }
}
