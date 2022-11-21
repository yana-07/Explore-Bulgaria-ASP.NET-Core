using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Categories;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using static ExploreBulgaria.Web.ViewModels.Common.Constants;

namespace ExploreBulgaria.Web.ViewModels.Attractions
{
    public class AddAttractionViewModel 
    {
        [Required(ErrorMessage = FieldRequired)]
        [StringLength(120, MinimumLength = 5, ErrorMessage = FieldLength)]
        [Display(Name = "Заглавие")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        [StringLength(10000, MinimumLength = 50, ErrorMessage = FieldLength)]
        [Display(Name = "Описание")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = FieldLength)]
        [Display(Name = "Регион")]
        public string Region { get; set; } = null!;

        [Required(ErrorMessage = FieldRequired)]
        public string CategoryId { get; set; } = null!;

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public IEnumerable<CategoryOptionViewModel> Categories { get; set; } 
            = Enumerable.Empty<CategoryOptionViewModel>();

        public IEnumerable<IFormFile> Images { get; set; } 
            = Enumerable.Empty<IFormFile>();
    }
}
