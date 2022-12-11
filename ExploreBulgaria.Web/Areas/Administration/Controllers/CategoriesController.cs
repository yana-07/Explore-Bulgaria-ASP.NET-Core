using ExploreBulgaria.Services.Data.Administration;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Web.ViewModels.Administration;
using Microsoft.AspNetCore.Mvc;
using static ExploreBulgaria.Services.Constants.MessageConstants;

namespace ExploreBulgaria.Web.Areas.Administration.Controllers
{
    public class CategoriesController : BaseController
    {
        private readonly IAdminService adminService;
        private readonly ILogger<CategoriesController> logger;

        public CategoriesController(
            IAdminService adminService,
            ILogger<CategoriesController> logger)
        {
            this.adminService = adminService;
            this.logger = logger;
        }

        public IActionResult Add()
        {
            var model = new AddCategoryViewModel();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await adminService.AddCategoryAsync(model.Name);
            }
            catch (ExploreBulgariaException ex)
            {
                ModelState.AddModelError(nameof(model.Name), ex.Message.ToString());

                return View(model);
            }
            catch (DbException ex)
            {
                TempData[ErrorMessage] = ex.Message.ToString();

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            TempData[SuccessMessage] = CategorySuccessfullyAdded;

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
