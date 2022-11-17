using System.ComponentModel.DataAnnotations;

namespace ExploreBulgaria.Web.ViewModels.Votes
{
    public class VoteInputModel
    {
        [Required]
        public string AttractionId { get; set; } = null!;

        [Range(1,5)]
        public byte Value { get; set; }
    }
}
