namespace ExploreBulgaria.Services
{
    public interface IAttractionModel
    {
        public string Name { get; set; }

        public string CategoryName { get; set; }

        public string? SubcategoryName { get; set; }

        public string RegionName { get; set; }

        public string? LocationName { get; set; }
    }
}
