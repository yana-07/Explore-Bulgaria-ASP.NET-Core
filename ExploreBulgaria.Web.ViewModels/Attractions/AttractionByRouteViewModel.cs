using ExploreBulgaria.Services;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionByRouteViewModel : IAttractionModel
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;

        public double DistanceFromStartPoint { get; set; }

        public double DistanceFromRoad { get; set; }

        public string? RemoteImageUrl { get; set; } 

        public string? BlobStorageUrl { get; set; }

        public string CategoryName { get; set; } = null!;

        public string? SubcategoryName { get; set; }

        public string RegionName { get; set; } = null!;

        public string? LocationName { get; set; }
    }
}
