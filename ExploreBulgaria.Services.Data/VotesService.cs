using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Web.ViewModels.Votes;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data
{
    public class VotesService : IVotesService
    {
        private readonly IRepository<Vote> repo;

        public VotesService(IRepository<Vote> repo)
        {
            this.repo = repo;
        }

        public async Task<double> GetAverageVoteAsync(string attractionId)
            => await repo.AllAsNoTracking()
                   .Where(v => v.AttractionId == attractionId)
                   .AverageAsync(v => v.Value);

        public async Task PostVoteAsync(VoteInputModel model, string visitorId)
        {
            var vote = await repo.All().FirstOrDefaultAsync(
                v => v.AttractionId == model.AttractionId &&
                v.AddedByVisitorId == visitorId);

            if (vote == null)
            {
                vote = new Vote
                {
                    AttractionId = model.AttractionId,
                    AddedByVisitorId = visitorId,
                    Value = model.Value
                };

                await repo.AddAsync(vote);
            }

            vote.Value = model.Value;

            await repo.SaveChangesAsync();
        }
    }
}
