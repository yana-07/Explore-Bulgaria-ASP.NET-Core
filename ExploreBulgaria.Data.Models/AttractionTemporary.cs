﻿using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants;
using static ExploreBulgaria.Data.Common.Constants.EntityAndVMConstants.Attraction;
using ExploreBulgaria.Data.Common.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExploreBulgaria.Data.Models
{
    public class AttractionTemporary : BaseDeletableModel<int>
    {
        [Required]
        [MaxLength(AttractionNameMaxLength)]
        public string Name { get; set; } = null!;

        [Required]
        [MaxLength(DescriptionMaxLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string CategoryId { get; set; } = null!;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        [Required]
        [MaxLength(AttractionNameMaxLength)]
        public string Region { get; set; } = null!;

        [MaxLength(NameMaxLength)]
        public string? Village { get; set; } = null!;

        [Required]
        [MaxLength(GuidMaxLength)]
        public string CreatedByVisitorId { get; set; } = null!;

        [Required]
        public string BlobNames { get; set; } = null!;

        public bool IsApproved { get; set; }

        public bool IsRejected { get; set; }
    }
}
