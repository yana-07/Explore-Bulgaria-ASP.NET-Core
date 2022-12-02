using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.ViewModels.Categories;
using ExploreBulgaria.Web.ViewModels.Locations;
using ExploreBulgaria.Web.ViewModels.Regions;
using ExploreBulgaria.Web.ViewModels.Subcategories;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttractionsApiController : ControllerBase
    {
        private readonly ICategoriesService categoriesService;
        private readonly ISubcategoriesService subcategoriesService;
        private readonly IRegionsService regionsService;
        private readonly ILocationsService locationsService;

        public AttractionsApiController(
            ICategoriesService categoriesService,
            ISubcategoriesService subcategoriesService,
            IRegionsService regionsService,
            ILocationsService locationsService)
        {
            this.categoriesService = categoriesService;
            this.subcategoriesService = subcategoriesService;
            this.regionsService = regionsService;
            this.locationsService = locationsService;
        }

        [HttpPost("subcategories")]
        public async Task<IActionResult> GetSubcategories(SubcategoryFilterViewModel model)
            => Ok(await this.subcategoriesService.GetAllForCategory<SubcategorySelectViewModel>(model.CategoryName));

        [HttpPost("regions")]
        public async Task<IActionResult> GetRegions(RegionFilterViewModel model)
            => Ok(await this.regionsService.GetAllForCategoryAndSubcategoryAsync<RegionSelectViewModel>(model.CategoryName, model.SubcategoryName));

        [HttpPost("categories")]
        public async Task<IActionResult> GetCategories(CategoryFilterViewModel model)
            => Ok(await this.categoriesService.GetAllForRegionAsync<CategorySelectViewModel>(model.RegionName));

        [HttpPost("locations")]
        public async Task<IActionResult> GetLocations(LocationFilterViewModel model)
            => Ok(await this.locationsService.GetAllForCategorySubcategoryAndRegionAsync<LocationSelectViewModel>(model.CategoryName, model.SubcategoryName, model.RegionName));
    }
}
