using Azure.Storage.Blobs;
using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Guards;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Attractions;
using ExploreBulgaria.Web.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;
using static ExploreBulgaria.Services.Constants.ExceptionConstants;

namespace ExploreBulgaria.Services.Data
{
    public class AttractionsService : IAttractionsService
    {
        private readonly IDeletableEnityRepository<Attraction> repo;
        private readonly IDeletableEnityRepository<AttractionTemporary> repoTemp;
        private readonly IGuard guard;
        private readonly ICategoriesService categoriesService;
        private readonly IDeletableEnityRepository<Visitor> visitorRepo;

        public AttractionsService(
            IDeletableEnityRepository<Attraction> repo,
            IDeletableEnityRepository<AttractionTemporary> repoTemp,
            IGuard guard,
            ICategoriesService categoriesService,
            IDeletableEnityRepository<Visitor> visitorRepo)
        {
            this.repo = repo;
            this.repoTemp = repoTemp;
            this.guard = guard;
            this.categoriesService = categoriesService;
            this.visitorRepo = visitorRepo;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(
            int page,
            AttractionsFilterModel filterModel,
            int itemsPerPage = 12)
        {
            var skip = (page - 1) * itemsPerPage;

            var attractions = ApplyFilter(
                filterModel.CategoryName, filterModel.SubcategoryName,
                filterModel.RegionName, filterModel.LocationName, filterModel.SearchTerm);

            return await attractions
                .To<T>()
                .Skip(skip)
                .Take(itemsPerPage)
                .ToListAsync();
        }

        public async Task<int> GetCountAsync(AttractionsFilterModel filterModel)
        {
            var attractions = ApplyFilter(
                filterModel.CategoryName, filterModel.SubcategoryName,
                filterModel.RegionName, filterModel.LocationName, filterModel.SearchTerm);

            return await attractions.CountAsync();
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            var attraction = await repo.AllAsNoTracking()
                 .Where(a => a.Id == id)
                 .To<T>()
                 .FirstOrDefaultAsync();

            guard.AgainstNull(attraction, InvalidAttractionId);

            return attraction!;
        }

        private IQueryable<Attraction> ApplyFilter(
            string? categoryName = null,
            string? subcategoryName = null,
            string? regionName = null,
            string? locationName = null,
            string? searchTerm = null)
        {
            var result = repo
                .AllAsNoTracking();

            if (string.IsNullOrEmpty(categoryName) == false)
            {
                result = result
                    .Where(a => a.Category.Name == categoryName);
            }

            if (string.IsNullOrEmpty(subcategoryName) == false)
            {
                result = result
                    .Where(a => a.Subcategory != null &&
                           a.Subcategory.Name == subcategoryName);
            }

            if (string.IsNullOrEmpty(regionName) == false)
            {
                result = result
                    .Where(a => a.Region.Name == regionName);
            }

            if (string.IsNullOrEmpty(locationName) == false)
            {
                result = result
                    .Where(a => a.Location != null &&
                           a.Location.Name == locationName);
            }

            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                result = result
                    .Where(a => EF.Functions.Like(a.Name.ToLower(), searchTerm) ||
                        EF.Functions.Like(a.Description.ToLower(), searchTerm));
            }

            return result;
        }       

        public async Task<IEnumerable<AttractionMineViewModel>> GetByVisitorIdAsync(string visitorId, int page, int itemsPerPage = 12)
        {
            var skip = (page - 1) * itemsPerPage;

            return await repoTemp.AllWithDeleted()
                .Where(a => a.CreatedByVisitorId == visitorId)
                .Select(a => new AttractionMineViewModel
                {
                    CategoryName = categoriesService
                       .GetByIdAsync<CategorySelectViewModel>(a.CategoryId)
                       .GetAwaiter().GetResult().Name,
                    Description = a.Description,
                    ImagesCount = a.BlobNames.Split(", ", StringSplitOptions.RemoveEmptyEntries).Length,
                    IsApproved = a.IsApproved,
                    IsRejected = a.IsRejected,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    Name = a.Name,
                    Region = a.Region,
                    CreatedOn = a.CreatedOn,    
                })
                .Skip(skip)
                .Take(itemsPerPage)
                .ToListAsync();
        }

        public async Task<int> GetCountByVisitorIdAsync(string visitorId)
        {
            return await repoTemp
                .AllWithDeleted()
                .Where(a => a.CreatedByVisitorId == visitorId)
                .CountAsync();
        }

        public async Task AddAttractionToFavorites(string visitorId, string attractionId)
        {
            var visitor = await visitorRepo
                .All()
                .Include(v => v.FavoriteAttractions)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await repo.GetByIdAsync(attractionId);
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

        public async Task AddAttractionToVisited(string visitorId, string attractionId)
        {
            var visitor = await visitorRepo.All()
                .Include(v => v.VisitedAttractions)
                .Include(x => x.WantToVisitAttractions)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await repo.GetByIdAsync(attractionId);
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
            var visitor = await visitorRepo.All()
                .Include(v => v.WantToVisitAttractions)
                .Include(v => v.VisitedAttractions)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await repo.GetByIdAsync(attractionId);
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
            var visitor = await visitorRepo.All()
                .Include(v => v.FavoriteAttractions)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await repo.GetByIdAsync(attractionId);
            guard.AgainstNull(attraction, InvalidAttractionId);

            return visitor!.FavoriteAttractions.Any(a => a.AttractionId == attractionId);
        }

        public async Task<bool> IsAddedToVisited(string visitorId, string attractionId)
        {
            var visitor = await visitorRepo.All()
                .Include(v => v.VisitedAttractions)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await repo.GetByIdAsync(attractionId);
            guard.AgainstNull(attraction, InvalidAttractionId);

            return visitor!.VisitedAttractions.Any(a => a.AttractionId == attractionId);
        }

        public async Task<bool> WantToVisit(string visitorId, string attractionId)
        {
            var visitor = await visitorRepo.All()
                .Include(v => v.WantToVisitAttractions)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await repo.GetByIdAsync(attractionId);
            guard.AgainstNull(attraction, InvalidAttractionId);

            return visitor!.WantToVisitAttractions.Any(a => a.AttractionId == attractionId);
        }
    }
}
