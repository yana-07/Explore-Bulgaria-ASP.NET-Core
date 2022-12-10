using AutoMapper;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionSimpleViewModel : IAttractionModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public string? SubcategoryName { get; set; }

        public string RegionName { get; set; } = null!;

        public string? VillageName { get; set; }

        public string? Description { get; set; }
                     
        public double? Latitude { get; set; }
                     
        public double? Longitude { get; set; }

        public bool? IsApproved { get; set; }

        public bool? IsRejected { get; set; }

        public DateTime? CreatedOn { get; set; }

        public IEnumerable<string> RemoteImageUrls { get; set; }
            = Enumerable.Empty<string>();

        public IEnumerable<string> BlobStorageUrls { get; set; }
            = Enumerable.Empty<string>();
    }
}
