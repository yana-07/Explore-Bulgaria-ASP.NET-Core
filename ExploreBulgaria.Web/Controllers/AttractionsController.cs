using Azure.Storage.Blobs;
using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Services.Extensions;
using ExploreBulgaria.Web.Extensions;
using ExploreBulgaria.Web.ViewModels.Attractions;
using ExploreBulgaria.Web.ViewModels.Categories;
using ExploreBulgaria.Web.ViewModels.Villages;
using ExploreBulgaria.Web.ViewModels.Regions;
using ExploreBulgaria.Web.ViewModels.Subcategories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using static ExploreBulgaria.Services.Constants.MessageConstants;

namespace ExploreBulgaria.Web.Controllers
{
    public class AttractionsController : BaseController
    {
        private readonly IAttractionsService attractionsService;
        private readonly ITemporaryAttractionsService temporaryAttractionsService;
        private readonly ICategoriesService categoriesService;
        private readonly ISubcategoriesService subcategoriesService;
        private readonly IRegionsService regionsService;
        private readonly IVillagesService villagesService;
        private readonly IVisitorsService visitorsService;
        private readonly BlobServiceClient blobServiceClient;
        private const int ItemsPerPage = 12;

        public AttractionsController(
            IAttractionsService attractionsService,
            ITemporaryAttractionsService temporaryAttractionsService,
            ICategoriesService categoriesService,
            ISubcategoriesService subcategoriesService,
            IRegionsService regionsService,
            IVillagesService villagesService,
            IVisitorsService visitorsService,
            BlobServiceClient blobServiceClient)
        {
            this.attractionsService = attractionsService;
            this.temporaryAttractionsService = temporaryAttractionsService;
            this.categoriesService = categoriesService;
            this.subcategoriesService = subcategoriesService;
            this.regionsService = regionsService;
            this.villagesService = villagesService;
            this.visitorsService = visitorsService;
            this.blobServiceClient = blobServiceClient;
        }

        [AllowAnonymous]
        public async Task <IActionResult> All(AttractionFilterModel filterModel, int page = 1)
        {
            if (page <= 0)
            {
                return NotFound();
            }

            var model = new AttractionListViewModel
            {
                PageNumber = page,
                ItemsPerPage = ItemsPerPage,
                FilterModel = new AttractionFilterModel
                {                   
                    Categories = await categoriesService.GetAllAsync<CategorySelectViewModel>(),
                    Subcategories = await subcategoriesService.GetAllAsync<SubcategorySelectViewModel>(),
                    Regions = await regionsService.GetAllAsync<RegionSelectViewModel>(),
                    Villages = await villagesService.GetAllAsync<VillageSelectViewModel>()
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
            model.FilterModel.VillageName = filterModel.VillageName;
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

                attraction.AddedToFavorites = await attractionsService.IsAddedToFavoritesAsync(User.VisitorId(), attraction.Id);
                attraction.AddedToVisited = await attractionsService.IsAddedToVisitedAsync(User.VisitorId(), attraction.Id);
                attraction.WantToVisit = await attractionsService.WantToVisitAsync(User.VisitorId(), attraction.Id);

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
                await temporaryAttractionsService.SaveTemporaryAsync(model, User.VisitorId());
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

        public async Task<IActionResult> Mine(AttractionVisitorFilterModel filterModel, int page = 1)
        {
            var visitorId = User.VisitorId();

            var model = new AttractionVisitorListViewModel
            {
                PageNumber = page,
                ItemsCount = await attractionsService.GetCountByVisitorIdAsync(visitorId),
                ItemsPerPage = ItemsPerPage,
                Attractions = await attractionsService.GetByVisitorIdAsync(visitorId, page, ItemsPerPage),
                FilterModel = filterModel,
                Area = "",
                Controller = "Attractions",
                Action = "Mine"
            };

            return View(model);
        }

        public async Task<IActionResult> Favorites(AttractionVisitorFilterModel filterModel, int page = 1)
        {
            var visitorId = User.VisitorId();

            var model = new AttractionVisitorListViewModel
            {
                Attractions = await attractionsService
                .GetFavoritesByVisitorIdAsync(visitorId, page, ItemsPerPage),
                PageNumber = page,
                ItemsCount = await attractionsService.GetWanToVisitByVisitorIdCount(visitorId),
                ItemsPerPage = ItemsPerPage,
                FilterModel = filterModel,
                Area = "",
                Controller = "Attractions",
                Action = "Favorites"              
            };

            return View(model);
        }

        public async Task<IActionResult> WantToVisit(AttractionVisitorFilterModel filterModel, int page = 1)
        {
            var visitorId = User.VisitorId();

            var model = new AttractionVisitorListViewModel
            {
                Attractions = await attractionsService
                .GetWantToVisitByVisitorIdAsync(visitorId, page, ItemsPerPage),
                PageNumber = page,
                ItemsCount = await attractionsService.GetWanToVisitByVisitorIdCount(visitorId),
                ItemsPerPage = ItemsPerPage,
                FilterModel = filterModel,
                Area = "",
                Controller = "Attractions",
                Action = "WantToVisit"
            };

            return View(model);
        }

        public async Task<IActionResult> Visited(AttractionVisitorFilterModel filterModel, int page = 1)
        {
            var visitorId = User.VisitorId();

            var model = new AttractionVisitorListViewModel
            {
                Attractions = await attractionsService
                .GetVisitedByVisitorIdAsync(User.VisitorId(), page, ItemsPerPage),
                PageNumber = page,
                ItemsCount = await attractionsService.GetVisitedByVisitorIdCountAsync(visitorId),
                ItemsPerPage = ItemsPerPage,
                FilterModel = filterModel,
                Area = "",
                Controller = "Attractions",
                Action = "Visited"
            };

            return View(model);
        }

        public async Task<IActionResult> ByRoute()
        {
            var model = new ByRouteInputModel
            {
                Categories = await categoriesService.GetAllAsync<CategoryOptionViewModel>()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ByRoute(ByRouteInputModel model, int page = 1)
        {
            var viewModel = new AttractionByRouteListViewModel
            {
                Attractions = await attractionsService
                   .GetByRouteAndCategoriesAsync(model.Steps, model.CategoriesInput, page, ItemsPerPage),
                Steps = model.Steps,
                PagesCount = (int)Math.Ceiling((decimal)(await attractionsService
                   .GetCountByRouteAndCategoriesAsync(model.Steps, model.CategoriesInput)) / ItemsPerPage),
                CategoriesInput = model.CategoriesInput,
                Page = page,
            };

            return View("VisualizeByRoute", viewModel);
        }

        public async Task<IActionResult> GetImage(string blobName)
        {
            var containerClient = blobServiceClient.GetBlobContainerClient("attractions");
            var blobClient = containerClient.GetBlobClient(blobName);

            var result = await blobClient.DownloadAsync();

            return File(result.Value.Content, result.Value.ContentType);
        }
    }
}
