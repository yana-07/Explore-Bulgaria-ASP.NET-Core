using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.Common;
using ExploreBulgaria.Web.ViewModels.Votes;
using Microsoft.AspNetCore.Mvc;

namespace ExploreBulgaria.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VotesController : ControllerBase
    {
        private readonly IVotesService votesService;

        public VotesController(IVotesService votesService)
        {
            this.votesService = votesService;
        }

        [HttpPost("postVote")]
        public async Task<double> PostVote(VoteInputModel model)
        {
            var visitorId = User.VisitorId();

            await votesService.PostVoteAsync(model, visitorId);

            return await votesService.GetAverageVoteAsync(model.AttractionId);
        }
    }
}
