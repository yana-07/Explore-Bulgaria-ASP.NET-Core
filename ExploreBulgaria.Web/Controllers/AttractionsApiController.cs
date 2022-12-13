using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.Extensions;
using ExploreBulgaria.Web.ViewModels.Attractions;
using ExploreBulgaria.Web.ViewModels.Categories;
using ExploreBulgaria.Web.ViewModels.Villages;
using ExploreBulgaria.Web.ViewModels.Regions;
using ExploreBulgaria.Web.ViewModels.Subcategories;
using Microsoft.AspNetCore.Mvc;
using ExploreBulgaria.Services.Exceptions;
using Microsoft.Extensions.Logging;

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
        private readonly IVillagesService locationsService;
        private readonly ILogger<AttractionsApiController> logger;

        public AttractionsApiController(
            IAttractionsService attractionsService, 
            ICategoriesService categoriesService,
            ISubcategoriesService subcategoriesService,
            IRegionsService regionsService,
            IVillagesService locationsService,
            ILogger<AttractionsApiController> logger)
        {
            this.attractionsService = attractionsService;
            this.categoriesService = categoriesService;
            this.subcategoriesService = subcategoriesService;
            this.regionsService = regionsService;
            this.locationsService = locationsService;
            this.logger = logger;
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

        [HttpPost("villages")]
        public async Task<IActionResult> GetVillages(VillageFilterViewModel model)
            => Ok(await this.locationsService.GetAllForCategorySubcategoryAndRegionAsync<VillageSelectViewModel>(model.CategoryName, model.SubcategoryName, model.RegionName));

        [HttpPost("addToFavorites")]
        public async Task<IActionResult> AddAttractionToFavorites(AttractionIdInputModel model)
        {
            try
            {
                await attractionsService
                    .AddAttractionToFavoritesAsync(User.VisitorId(), model.AttractionId);
            }
            catch (ExploreBulgariaException ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
            catch (ExploreBulgariaDbException ex)
            {
                logger.LogError(ex.InnerException, ex.Message.ToString());
                return StatusCode(StatusCodes
                    .Status500InternalServerError, new { message = ex.Message.ToString() });
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
            catch (ExploreBulgariaException ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
            catch (ExploreBulgariaDbException ex)
            {
                logger.LogError(ex.InnerException, ex.Message.ToString());
                return StatusCode(StatusCodes
                    .Status500InternalServerError, new { message = ex.Message.ToString() });
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
            catch (ExploreBulgariaException ex)
            {
                return BadRequest(new { message = ex.Message.ToString() });
            }
            catch (ExploreBulgariaDbException ex)
            {
                logger.LogError(ex.InnerException, ex.Message.ToString());
                return StatusCode(StatusCodes
                    .Status500InternalServerError, new { message = ex.Message.ToString() });
            }

            return NoContent();
        }
    }
}
