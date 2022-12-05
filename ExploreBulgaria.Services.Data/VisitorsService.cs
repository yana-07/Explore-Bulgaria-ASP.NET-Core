using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Guards;
using Microsoft.EntityFrameworkCore;
using static ExploreBulgaria.Services.Constants.ExceptionConstants;

namespace ExploreBulgaria.Services.Data
{
    public class VisitorsService : IVisitorsService
    {
        private readonly IDeletableEnityRepository<Visitor> repo;
        private readonly IDeletableEnityRepository<Attraction> attractionRepo;
        private readonly IGuard guard;

        public VisitorsService(
            IDeletableEnityRepository<Visitor> repo,
            IDeletableEnityRepository<Attraction> attractionRepo,
            IGuard guard)
        {
            this.repo = repo;
            this.attractionRepo = attractionRepo;
            this.guard = guard;
        }

        public async Task AddAttractionToFavorites(string visitorId, string attractionId)
        {
            var visitor = await repo
                .All()
                .Include(v => v.FavoriteAttractions)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await attractionRepo.GetByIdAsync(attractionId);
            guard.AgainstNull(attraction, InvalidAttractionId);

            if (visitor != null)
            {
                var favoriteAttraction = visitor
                    .FavoriteAttractions
                    .FirstOrDefault(fa => fa.AttractionId == attractionId);

                if (favoriteAttraction != null)
                {
                    visitor.FavoriteAttractions.Remove(favoriteAttraction);
                    await repo.SaveChangesAsync();
                    return;
                }

                visitor.FavoriteAttractions.Add(new VisitorFavoriteAttraction
                {
                    VisitorId = visitorId,
                    AttractionId = attractionId
                });

                await repo.SaveChangesAsync();
            }
        }

        public async Task<string> CreateByUserId(string userId)
        {
            var visitor = new Visitor
            {
                UserId = userId
            };

            await repo.AddAsync(visitor);

            await repo.SaveChangesAsync();

            return visitor.Id;
        }

        public async Task AddAttractionToVisited(string visitorId, string attractionId)
        {
            var visitor = await repo.All()
                .Include(v => v.VisitedAttractions)
                .Include(x => x.WantToVisitAttractions)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await attractionRepo.GetByIdAsync(attractionId);
            guard.AgainstNull(attraction, InvalidAttractionId);

            if (visitor != null)
            {
                var visitedAttraction = visitor
                    .VisitedAttractions
                    .FirstOrDefault(a => a.AttractionId == attractionId);

                if (visitedAttraction != null)
                {
                    visitor.VisitedAttractions.Remove(visitedAttraction);
                    await repo.SaveChangesAsync();
                    return;
                }

                var wantToVisitAttraction = visitor
                    .WantToVisitAttractions
                    .FirstOrDefault(a => a.AttractionId == attractionId);

                if (wantToVisitAttraction != null)
                {
                    visitor.WantToVisitAttractions.Remove(wantToVisitAttraction);
                }

                visitor.VisitedAttractions.Add(new VisitorVisitedAttraction
                {
                    VisitorId = visitorId,
                    AttractionId = attractionId
                });

                await repo.SaveChangesAsync();
            }
        }

        public async Task WantToVisitAttraction(string visitorId, string attractionId)
        {
            var visitor = await repo.All()
                .Include(v => v.WantToVisitAttractions)
                .Include(v => v.VisitedAttractions)        
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await attractionRepo.GetByIdAsync(attractionId);
            guard.AgainstNull(attraction, InvalidAttractionId);

            if (visitor != null)
            {
                var wantToVisitAttraction = visitor
                    .WantToVisitAttractions
                    .FirstOrDefault(a => a.AttractionId == attractionId);

                if (wantToVisitAttraction != null)
                {
                    visitor.WantToVisitAttractions.Remove(wantToVisitAttraction);
                    await repo.SaveChangesAsync();
                    return;
                }

                var visitedAttraction = visitor
                    .VisitedAttractions
                    .FirstOrDefault(va => va.AttractionId == attractionId);

                if (visitedAttraction != null)
                {
                    visitor.VisitedAttractions.Remove(visitedAttraction);
                }

                visitor.WantToVisitAttractions.Add(new VisitorWantToVisitAttraction
                {
                    VisitorId = visitorId,
                    AttractionId = attractionId
                });

                await repo.SaveChangesAsync();
            } 
        }

        public async Task<bool> IsAddedToFavorites(string visitorId, string attractionId)
        {
            var visitor = await repo.All()
                .Include(v => v.FavoriteAttractions)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await attractionRepo.GetByIdAsync(attractionId);
            guard.AgainstNull(attraction, InvalidAttractionId);

            return visitor!.FavoriteAttractions.Any(a => a.AttractionId == attractionId);
        }

        public async Task<bool> IsAddedToVisited(string visitorId, string attractionId)
        {
            var visitor = await repo.All()
                .Include(v => v.VisitedAttractions)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await attractionRepo.GetByIdAsync(attractionId);
            guard.AgainstNull(attraction, InvalidAttractionId);

            return visitor!.VisitedAttractions.Any(a => a.AttractionId == attractionId);
        }

        public async Task<bool> WantToVisit(string visitorId, string attractionId)
        {
            var visitor = await repo.All()
                .Include(v => v.WantToVisitAttractions)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await attractionRepo.GetByIdAsync(attractionId);
            guard.AgainstNull(attraction, InvalidAttractionId);

            return visitor!.WantToVisitAttractions.Any(a => a.AttractionId == attractionId);
        }
    }
}
