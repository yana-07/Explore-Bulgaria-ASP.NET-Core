using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.ViewModels.Attractions;
using ExploreBulgaria.Web.ViewModels.Categories;
using ExploreBulgaria.Web.ViewModels.Regions;
using ExploreBulgaria.Web.ViewModels.Subcategories;
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
        public async Task <IActionResult> All(AttractionsFilterModel filterModel, int id = 1)
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
                FilterModel = new AttractionsFilterModel
                {                   
                    Categories = await categoriesService.GetAllAsync<CategorySelectViewModel>(),
                    Subcategories = await subcategoriesService.GetAllAsync<SubcategorySelectViewModel>(),
                    Regions = await regionsService.GetAllAsync<RegionSelectViewModel>()
                }
            };

            model.Attractions = await attractionsService
                 .GetAllAsync<AttractionInListViewModel>(id,
                 filterModel,
                 ItemsPerPage);

            model.ItemsCount = await attractionsService.GetCountAsync(filterModel);

            model.FilterModel.CategoryName = filterModel.CategoryName;
            model.FilterModel.SubcategoryName = filterModel.SubcategoryName;
            model.FilterModel.RegionName = filterModel.RegionName;
            model.FilterModel.SearchTerm = filterModel.SearchTerm;

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(string attractionId)
        {
            var attraction = await attractionsService
                .GetByIdAsync<AttractionDetailsViewModel>(attractionId);

            return View(attraction);
        }

        public async Task<IActionResult> Add()
        {
            var model = new AddAttractionViewModel
            {
                Categories = await categoriesService
                    .GetAllAsync<CategoryOptionViewModel>()               
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddAttractionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await categoriesService
                    .GetAllAsync<CategoryOptionViewModel>();

                return View(model);
            }

            return RedirectToAction(nameof(All));
        }
    }
}
