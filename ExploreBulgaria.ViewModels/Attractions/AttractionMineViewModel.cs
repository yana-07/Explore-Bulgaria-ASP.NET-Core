namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AttractionMineViewModel
    {
        public string Name { get; set; } = null!;

        public string Region { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }

        public int ImagesCount { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
