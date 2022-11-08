﻿using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class RegionSelectViewModel : IMapFrom<Region>
    {
        public string Id { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}