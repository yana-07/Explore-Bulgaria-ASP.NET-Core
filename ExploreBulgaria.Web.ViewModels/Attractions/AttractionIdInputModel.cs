using System.ComponentModel.DataAnnotations;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionIdInputModel
    {
        [Required]
        public string AttractionId { get; set; } = null!;
    }
}
