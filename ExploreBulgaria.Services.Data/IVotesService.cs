using ExploreBulgaria.Web.ViewModels.Votes;

namespace ExploreBulgaria.Services.Data
{
    public interface IVotesService
    {
        Task PostVoteAsync(VoteInputModel model, string visitorId);

        Task<double> GetAverageVoteAsync(string attractionId);
    }
}
