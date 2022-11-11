using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.ViewModels.Categories;
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

        public AttractionsApiController(
            ICategoriesService categoriesService,
            ISubcategoriesService subcategoriesService,
            IRegionsService regionsService)
        {
            this.categoriesService = categoriesService;
            this.subcategoriesService = subcategoriesService;
            this.regionsService = regionsService;
        }

        [HttpPost("subcategories")]
        public async Task<ActionResult<IEnumerable<SubcategorySelectViewModel>>> GetSubcategoriesForCategory(SubcategoryFilterViewModel model)
            => Ok(await this.subcategoriesService.GetAllForCategoryAndRegionAsync<SubcategorySelectViewModel>(model.CategoryName, model.RegionName));

        [HttpPost("regions")]
        public async Task<ActionResult<IEnumerable<RegionSelectViewModel>>> GetRegionsForCategory(RegionFilterViewModel model)
            => Ok(await this.regionsService.GetAllForCategoryAndSubcategoryAsync<RegionSelectViewModel>(model.CategoryName, model.SubcategoryName));

        [HttpPost("categories")]
        public async Task<ActionResult<IEnumerable<CategorySelectViewModel>>> GetCategoriesForRegion(CategoryFilterViewModel model)
            => Ok(await this.categoriesService.GetAllForRegionAsync<CategorySelectViewModel>(model.RegionName));

    }
}
