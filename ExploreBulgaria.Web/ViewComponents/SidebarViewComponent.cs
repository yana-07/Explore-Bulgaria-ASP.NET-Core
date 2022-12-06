using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Services.Enums;
using ExploreBulgaria.Web.ViewModels.Attractions;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IAttractionsService attractionsService;

        public SidebarViewComponent(IAttractionsService attractionsService)
        {
            this.attractionsService = attractionsService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new SidebarComponentViewModel
            {
                MostVisited = await attractionsService
                   .GetSidebarAttractions(SidebarOrderEnum.MostVisited),
                MostFavorite = await attractionsService
                  .GetSidebarAttractions(SidebarOrderEnum.MostFavorite),
                Newest = await attractionsService
                  .GetSidebarAttractions(SidebarOrderEnum.Newest)
            };
        
            return View(model);
        }
    }
}
