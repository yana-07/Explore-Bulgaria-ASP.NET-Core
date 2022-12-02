using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Attractions;

namespace ExploreBulgaria.Web.ViewModels.Administration
{
    public class AttractionTemporaryViewModel : IMapFrom<AttractionTemporary>
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string Region { get; set; } = null!;

        public string CategoryId { get; set; } = null!;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string CreatedByVisitorId { get; set; } = null!;

        public string BlobNames { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
    }
}
