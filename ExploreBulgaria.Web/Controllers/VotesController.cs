using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Web.Extensions;
using ExploreBulgaria.Web.ViewModels.Votes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVotesService votesService;
        private readonly ILogger<VotesController> logger;

        public VotesController(
            IVotesService votesService,
            ILogger<VotesController> logger)
        {
            this.votesService = votesService;
            this.logger = logger;
        }

        [HttpPost("postVote")]
        [Authorize]
        public async Task<IActionResult> PostVote(VoteInputModel model)
        {
            var visitorId = User.VisitorId();

            try
            {
                await votesService.PostVoteAsync(model, visitorId);
            }
            catch (ExploreBulgariaDbException ex)
            {
                logger.LogError(ex.InnerException, ex.Message.ToString());
                return StatusCode(StatusCodes
                    .Status500InternalServerError, new { message = ex.Message.ToString() });
            }

            return Ok(await votesService.GetAverageVoteAsync(model.AttractionId));
        }
    }
}
