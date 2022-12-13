using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Web.ViewModels.Votes;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data.Tests.UnitTests
{
    public class VotesServiceTests : UnitTestsBase
    {
        IVotesService votesService;

        public override void SetUp()
        {
            base.SetUp();

            votesService = new VotesService(votesRepo);
        }

        [Test]
        public async Task GetAverageVoteAsync_ShouldWorkCorrectly()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            await votesRepo.AddAsync(new Vote
            {
                AddedByVisitorId = visitor!.Id,
                AttractionId = attraction!.Id,
                Value = 4
            });
            await votesRepo.AddAsync(new Vote
            {
                AddedByVisitorId = visitor!.Id,
                AttractionId = attraction!.Id,
                Value = 3
            });
            await votesRepo.SaveChangesAsync();

            var averageVote = await votesService.GetAverageVoteAsync(attraction.Id);

            Assert.That(averageVote, Is.EqualTo(3.5));
        }

        [Test]
        public async Task PostVoteAsync_ShouldPostVoteIfVisitorHasNotVotedBefore()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            var model = new VoteInputModel
            {
                AttractionId = attraction!.Id,
                Value = 3
            };

            await votesService.PostVoteAsync(model, visitor!.Id);

            var dbVote = await votesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            Assert.That(dbVote, Is.Not.Null);
            Assert.That(dbVote.Value, Is.EqualTo(model.Value));
            Assert.That(dbVote.AttractionId, Is.EqualTo(model.AttractionId));
        }

        [Test]
        public async Task PostVoteAsync_ShouldOverrideVoteValueIfVisitorHasAlreadyVoted()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            await votesRepo.AddAsync(new Vote
            {
                AddedByVisitorId = visitor!.Id,
                AttractionId = attraction!.Id,
                Value = 3
            });
            await votesRepo.SaveChangesAsync();

            var model = new VoteInputModel
            {
                AttractionId = attraction!.Id,
                Value = 4
            };

            await votesService.PostVoteAsync(model, visitor!.Id);

            var dbVote = await votesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            Assert.That(dbVote, Is.Not.Null);
            Assert.That(dbVote.Value, Is.EqualTo(model.Value));
            Assert.That(dbVote.AttractionId, Is.EqualTo(model.AttractionId));
        }
    }
}
