using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Services.Data.Administration;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Web.ViewModels.Administration;
using ExploreBulgaria.Web.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using static ExploreBulgaria.Services.Constants.ExceptionConstants;
using static ExploreBulgaria.Services.Constants.MessageConstants;

namespace ExploreBulgaria.Web.Areas.Administration.Controllers
{
    public class SubcategoriesController : BaseController
    {
        private readonly ICategoriesService categoriesService;
        private readonly IAdminService adminService;

        public SubcategoriesController(
            ICategoriesService categoriesService,
            IAdminService adminService)
        {
            this.categoriesService = categoriesService;
            this.adminService = adminService;
        }

        public async Task<IActionResult> Add()
        {
            var model = new AddSubcategoryViewModel
            {
                Categories = await categoriesService
                   .GetAllAsync<CategoryOptionViewModel>()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddSubcategoryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await categoriesService
                    .GetAllAsync<CategoryOptionViewModel>();  

                return View(model);
            }

            try
            {
                await adminService.AddSubcategoryAsync(model.Name, model.CategoryId);
            }
            catch (ExploreBulgariaException ex)
            {               
                if (ex.Message.ToString() == InvalidCategoryId)
                {
                    ModelState.AddModelError(nameof(model.CategoryId), ex.Message.ToString());
                }
                else
                {
                    ModelState.AddModelError(nameof(model.Name), ex.Message.ToString());
                }

                model.Categories = await categoriesService
                    .GetAllAsync<CategoryOptionViewModel>();  
                return View(model);
            }
            catch (ExploreBulgariaDbException ex)
            {
                TempData[ErrorMessage] = ex.Message.ToString();

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            TempData[SuccessMessage] = SubcategorySuccessfullyAdded;

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
