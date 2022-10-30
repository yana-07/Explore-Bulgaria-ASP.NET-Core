namespace ExploreBulgaria.Services.Models
{
    public class AttractionDto
    {
        public string CategoryName { get; set; } = null!;

        public string SubCategoryName { get; set; } = null!;

        public string AreaName { get; set; } = null!;

        public string AttractionName { get; set; } = null!;

        public string Location { get; set; } = null!;

        public string? Altitutude { get; set; }

        public string Description { get; set; } = null!;

        public string Latitude { get; set; } = null!;

        public string Longitude { get; set; } = null!;

        public List<string> ImagesUrls { get; set; } = new List<string>();
    }
}
