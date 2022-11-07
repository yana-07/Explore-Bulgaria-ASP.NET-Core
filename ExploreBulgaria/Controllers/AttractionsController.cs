using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.ViewModels.Attractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Controllers
{
    public class AttractionsController : BaseController
    {
        private readonly IAttractionsService attractionsService;
        private readonly ICategoriesService categoriesService;
        private readonly ISubcategoriesService subcategoriesService;
        private readonly IRegionsService regionsService;

        public AttractionsController(
            IAttractionsService attractionsService,
            ICategoriesService categoriesService,
            ISubcategoriesService subcategoriesService,
            IRegionsService regionsService)
        {
            this.attractionsService = attractionsService;
            this.categoriesService = categoriesService;
            this.subcategoriesService = subcategoriesService;
            this.regionsService = regionsService;
        }

        [AllowAnonymous]
        public async Task <IActionResult> All(int id = 1)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            const int ItemsPerPage = 12;
            var model = new AttractionsListViewModel
            {
                PageNumber = id,
                ItemsPerPage = ItemsPerPage,
                Attractions = await attractionsService
                     .GetAllAsync<AttractionInListViewModel>(id, ItemsPerPage),
                ItemsCount = attractionsService.GetCount(),
                Categories = await categoriesService.GetAllAsync<CategorySelectViewModel>(),
                Subcategories = await subcategoriesService.GetAllAsync<SubcategorySelectViewModel>(),
                Regions = await regionsService.GetAllAsync<RegionSelectViewModel>(),
            };

            return View(model);
        }
    }
}
