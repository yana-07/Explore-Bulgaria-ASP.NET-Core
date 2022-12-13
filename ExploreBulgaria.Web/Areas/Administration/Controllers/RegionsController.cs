using ExploreBulgaria.Services.Data.Administration;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Web.ViewModels.Administration;
using Microsoft.AspNetCore.Mvc;
using static ExploreBulgaria.Services.Constants.MessageConstants;

namespace ExploreBulgaria.Web.Areas.Administration.Controllers
{
    public class RegionsController : BaseController
    {
        private readonly IAdminService adminService;

        public RegionsController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        public IActionResult Add()
        {
            var model = new AddRegionViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await adminService.AddRegionAsync(model.Name);
            }
            catch (ExploreBulgariaException ex)
            {
                ModelState.AddModelError(nameof(model.Name), ex.Message.ToString());

                return View(model);
            }
            catch (ExploreBulgariaDbException ex)
            {
                TempData[ErrorMessage] = ex.Message.ToString();

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            TempData[SuccessMessage] = RegionSuccessfullyAdded;

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
