using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.Extensions;
using ExploreBulgaria.Web.ViewModels.Attractions;
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
        private readonly IAttractionsService attractionsService;
        private readonly ICategoriesService categoriesService;
        private readonly ISubcategoriesService subcategoriesService;
        private readonly IRegionsService regionsService;
        private readonly ILocationsService locationsService;

        public AttractionsApiController(
            IAttractionsService attractionsService, 
            ICategoriesService categoriesService,
            ISubcategoriesService subcategoriesService,
            IRegionsService regionsService,
            ILocationsService locationsService)
        {
            this.attractionsService = attractionsService;
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

        [HttpPost("addToFavorites")]
        public async Task<IActionResult> AddAttractionToFavorites(AttractionIdInputModel model)
        {
            try
            {
                await attractionsService
                    .AddAttractionToFavoritesAsync(User.VisitorId(), model.AttractionId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost("addToVisited")]
        public async Task<IActionResult> AddAttractionToVisited(AttractionIdInputModel model)
        {
            try
            {
                await attractionsService
                    .AddAttractionToVisitedAsync(User.VisitorId(), model.AttractionId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            return NoContent();
        }

        [HttpPost("wantToVisit")]
        public async Task<IActionResult> WantToVisitAttraction(AttractionIdInputModel model)
        {
            try
            {
                await attractionsService
                    .WantToVisitAttractionAsync(User.VisitorId(), model.AttractionId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}
