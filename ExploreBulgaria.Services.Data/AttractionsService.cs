using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Enums;
using ExploreBulgaria.Services.Guards;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Attractions;
using ExploreBulgaria.Web.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;
using static ExploreBulgaria.Services.Constants.ExceptionConstants;
using System.Device.Location;

namespace ExploreBulgaria.Services.Data
{
    public class AttractionsService : IAttractionsService
    {
        private readonly IDeletableEnityRepository<Attraction> repo;
        private readonly IDeletableEnityRepository<AttractionTemporary> repoTemp;
        private readonly IGuard guard;
        private readonly ICategoriesService categoriesService;
        private readonly ISpatialDataService spatialDataService;
        private readonly IDeletableEnityRepository<Visitor> visitorRepo;

        public AttractionsService(
            IDeletableEnityRepository<Attraction> repo,
            IDeletableEnityRepository<AttractionTemporary> repoTemp,
            IGuard guard,
            ICategoriesService categoriesService,
            ISpatialDataService spatialDataService,
            IDeletableEnityRepository<Visitor> visitorRepo)
        {
            this.repo = repo;
            this.repoTemp = repoTemp;
            this.guard = guard;
            this.categoriesService = categoriesService;
            this.spatialDataService = spatialDataService;
            this.visitorRepo = visitorRepo;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(
            int page,
            AttractionFilterModel filterModel,
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

        public async Task<int> GetCountAsync(AttractionFilterModel filterModel)
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

        public async Task<IEnumerable<AttractionSimpleViewModel>> GetByVisitorIdAsync(
            string visitorId, int page, int itemsPerPage = 12)
        {
            var skip = (page - 1) * itemsPerPage;

            return await repoTemp.AllWithDeleted()
                .Where(a => a.CreatedByVisitorId == visitorId)
                .Select(a => new AttractionSimpleViewModel
                {
                    CategoryName = categoriesService
                       .GetByIdAsync<CategorySelectViewModel>(a.CategoryId)
                       .GetAwaiter().GetResult().Name,
                    Description = a.Description,
                    BlobStorageUrls = a.BlobNames.Split(',', ' ', StringSplitOptions.RemoveEmptyEntries),
                    IsApproved = a.IsApproved,
                    IsRejected = a.IsRejected,
                    Latitude = a.Latitude,
                    Longitude = a.Longitude,
                    Name = a.Name,
                    RegionName = a.Region,
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

        public async Task AddAttractionToFavoritesAsync(string visitorId, string attractionId)
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

        public async Task AddAttractionToVisitedAsync(string visitorId, string attractionId)
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

        public async Task WantToVisitAttractionAsync(string visitorId, string attractionId)
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

        public async Task<bool> IsAddedToFavoritesAsync(string visitorId, string attractionId)
        {
            var visitor = await visitorRepo.AllAsNoTracking()
                .Include(v => v.FavoriteAttractions)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await repo.GetByIdAsync(attractionId);
            guard.AgainstNull(attraction, InvalidAttractionId);

            return visitor!.FavoriteAttractions.Any(a => a.AttractionId == attractionId);
        }

        public async Task<bool> IsAddedToVisitedAsync(string visitorId, string attractionId)
        {
            var visitor = await visitorRepo.AllAsNoTracking()
                .Include(v => v.VisitedAttractions)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await repo.GetByIdAsync(attractionId);
            guard.AgainstNull(attraction, InvalidAttractionId);

            return visitor!.VisitedAttractions.Any(a => a.AttractionId == attractionId);
        }

        public async Task<bool> WantToVisitAsync(string visitorId, string attractionId)
        {
            var visitor = await visitorRepo.AllAsNoTracking()
                .Include(v => v.WantToVisitAttractions)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await repo.GetByIdAsync(attractionId);
            guard.AgainstNull(attraction, InvalidAttractionId);

            return visitor!.WantToVisitAttractions.Any(a => a.AttractionId == attractionId);
        }

        public async Task<IEnumerable<AttractionSimpleViewModel>> GetFavoritesByVisitorIdAsync(
            string visitorId, int page, int itemsPerPage = 12)
        {
            var visitor = await visitorRepo.AllAsNoTracking()
                .Include(v => v.FavoriteAttractions).ThenInclude(fa => fa.Attraction)
                .Include(v => v.FavoriteAttractions).ThenInclude(fa => fa.Attraction.Location)
                .Include(v => v.FavoriteAttractions).ThenInclude(fa => fa.Attraction.Region)
                .Include(v => v.FavoriteAttractions).ThenInclude(fa => fa.Attraction.Category)
                .Include(v => v.FavoriteAttractions).ThenInclude(fa => fa.Attraction.Subcategory)
                .Include(v => v.FavoriteAttractions).ThenInclude(fa => fa.Attraction.Images)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var skip = (page - 1) * itemsPerPage;
            return visitor!.FavoriteAttractions
                .Select(fa => new AttractionSimpleViewModel
                {
                    Name = fa.Attraction.Name,
                    CategoryName = fa.Attraction.Category.Name,
                    SubcategoryName = fa.Attraction.Subcategory?.Name,
                    RegionName = fa.Attraction.Region.Name,
                    LocationName = fa.Attraction.Location?.Name,
                    Id = fa.Attraction.Id,
                    RemoteImageUrls = fa.Attraction.Images.Where(i => i.RemoteImageUrl != null).Select(i => i.RemoteImageUrl).Take(4)!,
                    BlobStorageUrls = fa.Attraction.Images.Where(i => i.BlobStorageUrl != null).Select(i => i.BlobStorageUrl).Take(4)!,
                })
                .Skip(skip)
                .Take(itemsPerPage);
        }

        public async Task<IEnumerable<AttractionSimpleViewModel>> GetVisitedByVisitorIdAsync(
            string visitorId, int page, int itemsPerPage = 12)
        {
            var visitor = await visitorRepo.AllAsNoTracking()
                .Include(v => v.VisitedAttractions).ThenInclude(fa => fa.Attraction)
                .Include(v => v.VisitedAttractions).ThenInclude(fa => fa.Attraction.Location)
                .Include(v => v.VisitedAttractions).ThenInclude(fa => fa.Attraction.Region)
                .Include(v => v.VisitedAttractions).ThenInclude(fa => fa.Attraction.Category)
                .Include(v => v.VisitedAttractions).ThenInclude(fa => fa.Attraction.Subcategory)
                .Include(v => v.VisitedAttractions).ThenInclude(fa => fa.Attraction.Images)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var skip = (page - 1) * itemsPerPage;
            return visitor!.VisitedAttractions
                .Select(fa => new AttractionSimpleViewModel
                {
                    Name = fa.Attraction.Name,
                    CategoryName = fa.Attraction.Category.Name,
                    SubcategoryName = fa.Attraction.Subcategory?.Name,
                    RegionName = fa.Attraction.Region.Name,
                    LocationName = fa.Attraction.Location?.Name,
                    Id = fa.Attraction.Id,
                    RemoteImageUrls = fa.Attraction.Images.Where(i => i.RemoteImageUrl != null).Select(i => i.RemoteImageUrl).Take(4)!,
                    BlobStorageUrls = fa.Attraction.Images.Where(i => i.BlobStorageUrl != null).Select(i => i.BlobStorageUrl).Take(4)!,
                })
                .Skip(skip)
                .Take(itemsPerPage);
        }

        public async Task<IEnumerable<AttractionSimpleViewModel>> GetWantToVisitByVisitorIdAsync(
            string visitorId, int page, int itemsPerPage = 12)
        {
            var visitor = await visitorRepo.AllAsNoTracking()
                .Include(v => v.WantToVisitAttractions).ThenInclude(fa => fa.Attraction)
                .Include(v => v.WantToVisitAttractions).ThenInclude(fa => fa.Attraction.Location)
                .Include(v => v.WantToVisitAttractions).ThenInclude(fa => fa.Attraction.Region)
                .Include(v => v.WantToVisitAttractions).ThenInclude(fa => fa.Attraction.Category)
                .Include(v => v.WantToVisitAttractions).ThenInclude(fa => fa.Attraction.Subcategory)
                .Include(v => v.WantToVisitAttractions).ThenInclude(fa => fa.Attraction.Images)
                .FirstOrDefaultAsync();
            guard.AgainstNull(visitor, InvalidVisitorId);

            var skip = (page - 1) * itemsPerPage;
            return visitor!.WantToVisitAttractions
                .Select(fa => new AttractionSimpleViewModel
                {
                    Name = fa.Attraction.Name,
                    CategoryName = fa.Attraction.Category.Name,
                    SubcategoryName = fa.Attraction.Subcategory?.Name,
                    RegionName = fa.Attraction.Region.Name,
                    LocationName = fa.Attraction.Location?.Name,
                    Id = fa.Attraction.Id,
                    RemoteImageUrls = fa.Attraction.Images.Where(i => i.RemoteImageUrl != null).Select(i => i.RemoteImageUrl).Take(4)!,
                    BlobStorageUrls = fa.Attraction.Images.Where(i => i.BlobStorageUrl != null).Select(i => i.BlobStorageUrl).Take(4)!,
                })
                .Skip(skip)
                .Take(itemsPerPage);
        }

        public async Task<int> GetFavoritesByVisitorIdCountAsync(string visitorId)
            => await visitorRepo.AllAsNoTracking()
                .Where(v => v.Id == visitorId)
                .Select(v => v.FavoriteAttractions)
                .CountAsync();

        public async Task<int> GetVisitedByVisitorIdCountAsync(string visitorId)
            => await visitorRepo.AllAsNoTracking()
                .Where(v => v.Id == visitorId)
                .Select(v => v.VisitedAttractions)
                .CountAsync();

        public async Task<int> GetWanToVisitByVisitorIdCount(string visitorId)
            => await visitorRepo.AllAsNoTracking()
                .Where(v => v.Id == visitorId)
                .Select(v => v.WantToVisitAttractions)
                .CountAsync();

        public async Task<IEnumerable<AttractionSidebarViewModel>> GetSidebarAttractions(SidebarOrderEnum orderBy)
        {
            var attractions = repo.AllAsNoTracking();

            if (orderBy == SidebarOrderEnum.MostVisited)
            {
                attractions = attractions
                    .Where(a => a.VisitedByVisitors.Any())
                    .OrderByDescending(a => a.VisitedByVisitors.Count);
            }
            else if (orderBy == SidebarOrderEnum.MostFavorite)
            {
                attractions = attractions
                    .Where(a => a.AddedToFavoritesByVisitors.Any())
                    .OrderByDescending(a => a.AddedToFavoritesByVisitors.Count);
            }
            else if (orderBy == SidebarOrderEnum.Newest)
            {
                attractions = attractions
                    .OrderByDescending(a => a.CreatedOn);
            }

            return await attractions
                .To<AttractionSidebarViewModel>()
                .Take(3)
                .ToListAsync();
        }

        public async Task<IEnumerable<T>> GetByCategories<T>(params string[] categoryIds)
            => await repo.AllAsNoTracking()
            .Where(a => categoryIds.Contains(a.CategoryId))
            .To<T>()
            .ToListAsync();

        public async Task<IEnumerable<AttractionByRouteViewModel>> GetByRouteAndCategoriesAsync(
            string coordinates,
            IEnumerable<string> categoryIds)
        {
            var points = spatialDataService
                .GetGeometryPointsByStringCoordinates(coordinates.Split(',', StringSplitOptions.RemoveEmptyEntries));

            var result = repo
                .AllAsNoTracking().Include(a => a.Images)
                .Include(a => a.Category).Include(a => a.Subcategory)
                .Include(a => a.Region).Include(a => a.Location)
                .Where(a => !categoryIds.Any() ? true : categoryIds.Contains(a.CategoryId));

            var attractions = await result.ToListAsync();

            return attractions
                .Where(a => points.Any(p =>
                {
                    var geoAttr = new GeoCoordinate(a.Coordinates.Y, a.Coordinates.X);

                    return p.GetDistanceTo(geoAttr) <= 15000 ? true : false;
                }))
                .Select(a =>
                {
                    var geo = new GeoCoordinate(a.Coordinates.Y, a.Coordinates.X);

                    return new AttractionByRouteViewModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        RemoteImageUrl = a.Images.Where(i => i.RemoteImageUrl != null).Select(i => i.RemoteImageUrl).FirstOrDefault()!,
                        BlobStorageUrl = a.Images.Where(i => i.BlobStorageUrl != null).Select(i => i.BlobStorageUrl).FirstOrDefault()!,
                        DistanceFromRoad = points.Min(p => p.GetDistanceTo(geo)),
                        DistanceFromStartPoint = geo.GetDistanceTo(points[0]),
                        CategoryName = a.Category.Name,
                        LocationName = a.Location?.Name,
                        RegionName = a.Region.Name,
                        SubcategoryName = a.Subcategory?.Name
                    };
                }).ToList();
        }
    }
}
