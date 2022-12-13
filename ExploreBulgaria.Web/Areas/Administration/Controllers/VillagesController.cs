using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Services.Data.Administration;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Web.ViewModels.Administration;
using ExploreBulgaria.Web.ViewModels.Regions;
using Microsoft.AspNetCore.Mvc;
using static ExploreBulgaria.Services.Constants.ExceptionConstants;
using static ExploreBulgaria.Services.Constants.MessageConstants;

namespace ExploreBulgaria.Web.Areas.Administration.Controllers
{
    public class VillagesController : BaseController
    {
        private readonly IRegionsService regionsService;
        private readonly IAdminService adminService;

        public VillagesController(
            IRegionsService regionsService,
            IAdminService adminService)
        {
            this.regionsService = regionsService;
            this.adminService = adminService;
        }

        public async Task<IActionResult> Add()
        {
            var model = new AddVillageViewModel
            {
                Regions = await regionsService
                   .GetAllAsync<RegionOptionViewModel>()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddVillageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Regions = await regionsService
                    .GetAllAsync<RegionOptionViewModel>();

                return View(model);
            }

            try
            {
                await adminService.AddVillageAsync(model.Name, model.RegionId);
            }
            catch (ExploreBulgariaException ex)
            {
                if (ex.Message.ToString() == InvalidRegionId)
                {
                    ModelState.AddModelError(nameof(model.RegionId), ex.Message.ToString());
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Name), ex.Message.ToString());
                }

                model.Regions = await regionsService
                   .GetAllAsync<RegionOptionViewModel>();
                return View(model);
            }
            catch (ExploreBulgariaDbException ex)
            {
                TempData[ErrorMessage] = ex.Message.ToString();

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            TempData[SuccessMessage] = VillageSuccessfullyAdded;

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
