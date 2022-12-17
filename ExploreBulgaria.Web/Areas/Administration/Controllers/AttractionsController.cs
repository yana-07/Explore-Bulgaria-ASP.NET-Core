using Azure.Storage.Blobs;
using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Services.Data.Administration;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Web.ViewModels.Administration;
using ExploreBulgaria.Web.ViewModels.Categories;
using ExploreBulgaria.Web.ViewModels.Regions;
using ExploreBulgaria.Web.ViewModels.Subcategories;
using ExploreBulgaria.Web.ViewModels.Villages;
using Microsoft.AspNetCore.Mvc;
using static ExploreBulgaria.Services.Constants.MessageConstants;

namespace ExploreBulgaria.Web.Areas.Administration.Controllers
{
    public class AttractionsController : BaseController
    {
        private readonly IAdminService adminService;
        private readonly ICategoriesService categoriesService;
        private readonly ISubcategoriesService subcategoriesService;
        private readonly IRegionsService regionsService;
        private readonly IVillagesService vilaggesService;
        private readonly ILogger<AttractionsController> logger;
        private const int ItemsPerPage = 12;

        public AttractionsController(
            IAdminService adminService,
            ICategoriesService categoriesService,
            ISubcategoriesService subcategoriesService,
            IRegionsService regionsService,
            IVillagesService vilaggesService,
            ILogger<AttractionsController> logger)
        {
            this.adminService = adminService;
            this.categoriesService = categoriesService;
            this.subcategoriesService = subcategoriesService;
            this.regionsService = regionsService;
            this.vilaggesService = vilaggesService;
            this.logger = logger;
        }

        public async Task<IActionResult> All(AttractionTemporaryFilterModel filterModel, int page = 1)
        {
            var model = new AttractionTemporaryListViewModel
            {
                PageNumber = page,
                ItemsPerPage = ItemsPerPage,
                ItemsCount = await adminService.GetCountAsync(filterModel),
                Attractions = await adminService
                   .GetAllAsync<AttractionTemporaryViewModel>(
                       page, filterModel, ItemsPerPage),
                FilterModel = filterModel,
                Area = "Administration",
                Controller = "Attractions",
                Action = "All"
            };

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var model = await adminService
                    .GetByIdAsync<AttractionTempDetailsViewModel>(id);

                await PopulateModel(model!);

                return View(model);
            }
            catch (Exception)
            {
                // TODO: Toast message
                return RedirectToAction(nameof(All));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Approve(AttractionTempDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateModel(model);

                return View(nameof(Details), model);
            }

            try
            {
                await adminService.ApproveAsync(model);
            }
            catch(Exception ex)
            {
                TempData[ErrorMessage] = ex.Message.ToString();
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id, string visitorId)
        {
            try
            {
                await adminService.RejectAsync(id, visitorId);
            }
            catch (Exception ex)
            {
                TempData[ErrorMessage] = ex.Message.ToString();
            }

            return RedirectToAction(nameof(All));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await adminService.DeleteAsync(id);
            }
            catch (ExploreBulgariaException ex)
            {
                TempData[ErrorMessage] = ex.Message.ToString();
            }
            catch (ExploreBulgariaDbException ex)
            {
                TempData[ErrorMessage] = ex.Message.ToString();
                logger.LogError(ex.InnerException, ex.Message.ToString());
            }
            
            return RedirectToAction(nameof(All), new { area = ""});
        }

        private async Task PopulateModel(AttractionTempDetailsViewModel model)
        {
            model.CategoryModel = await categoriesService
                .GetByIdAsync<CategoryOptionViewModel>(model.CategoryId);

            model.Categories = await categoriesService
                .GetAllAsync<CategoryOptionViewModel>();
            model.Subcategories = await subcategoriesService
                .GetAllForCategory<SubcategoryOptionViewModel>(model.CategoryModel!.Name);
            model.Regions = await regionsService
                .GetAllAsync<RegionOptionViewModel>();
            model.Villages = await vilaggesService
                .GetAllAsync<VillageOptionViewModel>();
        }
    }
}
