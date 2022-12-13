namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionHomePageViewModel
    {
        public string Name { get; set; } = null!;

        public string? RemoteImageUrl { get; set; }

        public string? BlobStorageImageUrl { get; set; }
    }
}
