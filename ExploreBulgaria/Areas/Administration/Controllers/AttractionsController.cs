using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.Controllers;
using ExploreBulgaria.Web.ViewModels.Administration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ExploreBulgaria.Common.GlobalConstants;

namespace ExploreBulgaria.Web.Areas.Administration.Controllers
{
    [Authorize(Roles = AdministratorRoleName)]
    [Area("Administration")]
    public class AttractionsController : BaseController
    {
        private readonly ITemporaryAttractionsService attractionsService;
        private const int ItemsPerPage = 1;

        public AttractionsController(ITemporaryAttractionsService attractionsService)
        {
            this.attractionsService = attractionsService;
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
    }
}
