using Azure.Storage.Blobs;
using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.ViewModels.Administration;
using ExploreBulgaria.Web.ViewModels.Categories;
using ExploreBulgaria.Web.ViewModels.Regions;
using ExploreBulgaria.Web.ViewModels.Subcategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ExploreBulgaria.Services.Common.Constants.GlobalConstants;

namespace ExploreBulgaria.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = AdministratorRoleName)]
    [Area("Administration")]
    public class AttractionsController : Controller
    {
        private readonly ITemporaryAttractionsService attractionsService;
        private readonly ICategoriesService categoriesService;
        private readonly ISubcategoriesService subcategoriesService;
        private readonly IRegionsService regionsService;
        private readonly BlobServiceClient blobServiceClient;
        private const int ItemsPerPage = 12;

        public AttractionsController(
            ITemporaryAttractionsService attractionsService,
            ICategoriesService categoriesService,
            ISubcategoriesService subcategoriesService,
            IRegionsService regionsService,
            BlobServiceClient blobContainerClient)
        {
            this.attractionsService = attractionsService;
            this.categoriesService = categoriesService;
            this.subcategoriesService = subcategoriesService;
            this.regionsService = regionsService;
            this.blobServiceClient = blobContainerClient;
        }

        public async Task<IActionResult> All(AttractionTemporaryFilterModel filterModel, int page = 1)
        {
            var model = new AttractionTemporaryListViewModel
            {
                PageNumber = page,
                ItemsPerPage = ItemsPerPage,
                ItemsCount = await attractionsService.GetCountAsync(filterModel),
                Attractions = await attractionsService
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
                var model = await attractionsService
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

        public async Task<IActionResult> GetImage(string blobName)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient("attractions");
            var blobClient = containerClient.GetBlobClient(blobName);

            var result = await blobClient.DownloadAsync();

            return File(result.Value.Content, result.Value.ContentType);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(AttractionTempDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateModel(model);

                return View(nameof(Details), model);
            }

            await attractionsService.ApproveAsync(model);

            return RedirectToAction(nameof(All));
        }

        private async Task PopulateModel(AttractionTempDetailsViewModel model)
        {
            model.CategoryModel = await categoriesService
                .GetByIdAsync<CategoryOptionViewModel>(model.CategoryId);

            model.Categories = await categoriesService
                .GetAllAsync<CategoryOptionViewModel>();
            model.Subcategories = await subcategoriesService
                .GetAllForCategoryAndRegionAsync<SubcategoryOptionViewModel>(model.CategoryModel!.Name);
            model.Regions = await regionsService
                .GetAllAsync<RegionOptionViewModel>();
        }
    }
}
