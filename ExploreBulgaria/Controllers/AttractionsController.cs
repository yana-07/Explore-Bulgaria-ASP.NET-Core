using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Services.Extensions;
using ExploreBulgaria.Web.Common;
using ExploreBulgaria.Web.ViewModels.Attractions;
using ExploreBulgaria.Web.ViewModels.Categories;
using ExploreBulgaria.Web.ViewModels.Locations;
using ExploreBulgaria.Web.ViewModels.Regions;
using ExploreBulgaria.Web.ViewModels.Subcategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ExploreBulgaria.Services.Common.Constants.MessageConstants;

namespace ExploreBulgaria.Web.Controllers
{
    public class AttractionsController : BaseController
    {
        private readonly IAttractionsService attractionsService;
        private readonly ICategoriesService categoriesService;
        private readonly ISubcategoriesService subcategoriesService;
        private readonly IRegionsService regionsService;
        private readonly ILocationsService locationsService;
        private const int ItemsPerPage = 12;

        public AttractionsController(
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

        [AllowAnonymous]
        public async Task <IActionResult> All(AttractionsFilterModel filterModel, int page = 1)
        {
            if (page <= 0)
            {
                return NotFound();
            }

            var model = new AttractionsListViewModel
            {
                PageNumber = page,
                ItemsPerPage = ItemsPerPage,
                FilterModel = new AttractionsFilterModel
                {                   
                    Categories = await categoriesService.GetAllAsync<CategorySelectViewModel>(),
                    Subcategories = await subcategoriesService.GetAllAsync<SubcategorySelectViewModel>(),
                    Regions = await regionsService.GetAllAsync<RegionSelectViewModel>(),
                    Locations = await locationsService.GetAllAsync<LocationSelectViewModel>()
                },
                Area = "",
                Controller = "Attractions",
                Action = "All"
            };

            model.Attractions = await attractionsService
                 .GetAllAsync<AttractionInListViewModel>(page,
                 filterModel,
                 ItemsPerPage);

            model.ItemsCount = await attractionsService.GetCountAsync(filterModel);

            model.FilterModel.CategoryName = filterModel.CategoryName;
            model.FilterModel.SubcategoryName = filterModel.SubcategoryName;
            model.FilterModel.RegionName = filterModel.RegionName;
            model.FilterModel.LocationName = filterModel.LocationName;
            model.FilterModel.SearchTerm = filterModel.SearchTerm;

            return View(model);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(string id, [FromRoute]string information)
        {
            try
            {
                var attraction = await attractionsService
                    .GetByIdAsync<AttractionDetailsViewModel>(id);

                if (information != attraction.GenerateSlug())
                {
                    TempData[ErrorMessage] = "Моля, не променяйте информацията в адреса!";

                    return RedirectToAction(nameof(All));
                }

                return View(attraction);
            }
            catch (Exception)
            {
                // TODO: Toast message
                return RedirectToAction(nameof(All));
            }
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

            try
            {
                await attractionsService.SaveTemporaryAsync(model, User.VisitorId());
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);

                model.Categories = await categoriesService
                    .GetAllAsync<CategoryOptionViewModel>();

                return View(model);
            }

            TempData[SuccessMessage] = "Благодарим Ви за предложението! Ще бъдете уведомени относно статуса на обекта.";

            return RedirectToAction(nameof(All));
        }

        //public async Task<IActionResult> Mine()
        //{
        //    var model = await attractionsService.GetByVisitorIdAsync<>(User.VisitorId());
        //}
    }
}
