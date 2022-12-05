using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.Extensions;
using ExploreBulgaria.Web.ViewModels.Attractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VisitorsApiController : ControllerBase
    {
        private readonly IVisitorsService visitorsService;

        public VisitorsApiController(IVisitorsService visitorsService)
        {
            this.visitorsService = visitorsService;
        }

        [HttpPost("addToFavorites")]
        public async Task<IActionResult> AddAttractionToFavorites(AttractionIdInputModel model)
        {
            try
            {
                await visitorsService
                    .AddAttractionToFavorites(User.VisitorId(), model.AttractionId);
            }
            catch (Exception ex)
            {
               return BadRequest(ex.Message);
            }

            return NoContent();
        }

        [HttpPost("addToVisited")]
        public async Task<IActionResult> AddAttractionToVisited(AttractionIdInputModel model)
        {
            try
            {
                await visitorsService
                    .AddAttractionToVisited(User.VisitorId(), model.AttractionId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


            return NoContent();
        }

        [HttpPost("wantToVisit")]
        public async Task<IActionResult> WantToVisitAttraction(AttractionIdInputModel model)
        {
            try
            {
                await visitorsService
                    .WantToVisitAttraction(User.VisitorId(), model.AttractionId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return NoContent();
        }
    }
}
