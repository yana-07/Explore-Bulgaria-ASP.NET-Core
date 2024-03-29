﻿using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Enums;
using ExploreBulgaria.Services.Guards;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Attractions;
using ExploreBulgaria.Web.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;
using System.Device.Location;
using ExploreBulgaria.Data;
using ExploreBulgaria.Services.Exceptions;
using static ExploreBulgaria.Services.Constants.ExceptionConstants;

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
        private readonly ApplicationDbContext context;

        public AttractionsService(
            IDeletableEnityRepository<Attraction> repo,
            IDeletableEnityRepository<AttractionTemporary> repoTemp,
            IGuard guard,
            ICategoriesService categoriesService,
            ISpatialDataService spatialDataService,
            IDeletableEnityRepository<Visitor> visitorRepo,
            ApplicationDbContext context)
        {
            this.repo = repo;
            this.repoTemp = repoTemp;
            this.guard = guard;
            this.categoriesService = categoriesService;
            this.spatialDataService = spatialDataService;
            this.visitorRepo = visitorRepo;
            this.context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(
            int page,
            AttractionFilterModel filterModel,
            int itemsPerPage = 12)
        {
            var skip = (page - 1) * itemsPerPage;

            var attractions = ApplyFilter(
                filterModel.CategoryName, filterModel.SubcategoryName,
                filterModel.RegionName, filterModel.VillageName, filterModel.SearchTerm);

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
                filterModel.RegionName, filterModel.VillageName, filterModel.SearchTerm);

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
                .FirstOrDefaultAsync(v => v.Id == visitorId);
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
                    try
                    {
                        await repo.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw new ExploreBulgariaDbException(SavingToDatabase, ex);
                    }

                    return;
                }

                visitor.FavoriteAttractions.Add(new VisitorFavoriteAttraction
                {
                    VisitorId = visitorId,
                    AttractionId = attractionId
                });

                try
                {
                    await repo.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new ExploreBulgariaDbException(SavingToDatabase, ex);
                }
            }
        }

        public async Task AddAttractionToVisitedAsync(string visitorId, string attractionId)
        {
            var visitor = await visitorRepo.All()
                .Include(v => v.VisitedAttractions)
                .Include(x => x.WantToVisitAttractions)
                .FirstOrDefaultAsync(v => v.Id == visitorId);
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
                    try
                    {
                        await repo.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw new ExploreBulgariaDbException(SavingToDatabase, ex);
                    }

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

                try
                {
                    await repo.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new ExploreBulgariaDbException(SavingToDatabase, ex);
                }
            }
        }

        public async Task WantToVisitAttractionAsync(string visitorId, string attractionId)
        {
            var visitor = await visitorRepo.All()
                .Include(v => v.WantToVisitAttractions)
                .Include(v => v.VisitedAttractions)
                .FirstOrDefaultAsync(v => v.Id == visitorId);
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
                    try
                    {
                        await repo.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        throw new ExploreBulgariaDbException(SavingToDatabase, ex);
                    }

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

                try
                {
                    await repo.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new ExploreBulgariaDbException(SavingToDatabase, ex);
                }
            }
        }

        public async Task<bool> IsAddedToFavoritesAsync(string visitorId, string attractionId)
        {
            var visitor = await visitorRepo.AllAsNoTracking()
                .Include(v => v.FavoriteAttractions)
                .FirstOrDefaultAsync(v => v.Id == visitorId);
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await repo.GetByIdAsync(attractionId);
            guard.AgainstNull(attraction, InvalidAttractionId);

            return visitor!.FavoriteAttractions.Any(a => a.AttractionId == attractionId);
        }

        public async Task<bool> IsAddedToVisitedAsync(string visitorId, string attractionId)
        {
            var visitor = await visitorRepo.AllAsNoTracking()
                .Include(v => v.VisitedAttractions)
                .FirstOrDefaultAsync(v => v.Id == visitorId);
            guard.AgainstNull(visitor, InvalidVisitorId);

            var attraction = await repo.GetByIdAsync(attractionId);
            guard.AgainstNull(attraction, InvalidAttractionId);

            return visitor!.VisitedAttractions.Any(a => a.AttractionId == attractionId);
        }

        public async Task<bool> WantToVisitAsync(string visitorId, string attractionId)
        {
            var visitor = await visitorRepo.AllAsNoTracking()
                .Include(v => v.WantToVisitAttractions)
                .FirstOrDefaultAsync(v => v.Id == visitorId);
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
                .Include(v => v.FavoriteAttractions).ThenInclude(fa => fa.Attraction.Village)
                .Include(v => v.FavoriteAttractions).ThenInclude(fa => fa.Attraction.Region)
                .Include(v => v.FavoriteAttractions).ThenInclude(fa => fa.Attraction.Category)
                .Include(v => v.FavoriteAttractions).ThenInclude(fa => fa.Attraction.Subcategory)
                .Include(v => v.FavoriteAttractions).ThenInclude(fa => fa.Attraction.Images)
                .FirstOrDefaultAsync(v => v.Id == visitorId);
            guard.AgainstNull(visitor, InvalidVisitorId);

            var skip = (page - 1) * itemsPerPage;
            return visitor!.FavoriteAttractions
                .Select(fa => new AttractionSimpleViewModel
                {
                    Name = fa.Attraction.Name,
                    CategoryName = fa.Attraction.Category.Name,
                    SubcategoryName = fa.Attraction.Subcategory?.Name,
                    RegionName = fa.Attraction.Region.Name,
                    VillageName = fa.Attraction.Village?.Name,
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
                .Include(v => v.VisitedAttractions).ThenInclude(fa => fa.Attraction.Village)
                .Include(v => v.VisitedAttractions).ThenInclude(fa => fa.Attraction.Region)
                .Include(v => v.VisitedAttractions).ThenInclude(fa => fa.Attraction.Category)
                .Include(v => v.VisitedAttractions).ThenInclude(fa => fa.Attraction.Subcategory)
                .Include(v => v.VisitedAttractions).ThenInclude(fa => fa.Attraction.Images)
                .FirstOrDefaultAsync(v => v.Id == visitorId);
            guard.AgainstNull(visitor, InvalidVisitorId);

            var skip = (page - 1) * itemsPerPage;
            return visitor!.VisitedAttractions
                .Select(fa => new AttractionSimpleViewModel
                {
                    Name = fa.Attraction.Name,
                    CategoryName = fa.Attraction.Category.Name,
                    SubcategoryName = fa.Attraction.Subcategory?.Name,
                    RegionName = fa.Attraction.Region.Name,
                    VillageName = fa.Attraction.Village?.Name,
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
                .Include(v => v.WantToVisitAttractions).ThenInclude(fa => fa.Attraction.Village)
                .Include(v => v.WantToVisitAttractions).ThenInclude(fa => fa.Attraction.Region)
                .Include(v => v.WantToVisitAttractions).ThenInclude(fa => fa.Attraction.Category)
                .Include(v => v.WantToVisitAttractions).ThenInclude(fa => fa.Attraction.Subcategory)
                .Include(v => v.WantToVisitAttractions).ThenInclude(fa => fa.Attraction.Images)
                .FirstOrDefaultAsync(v => v.Id == visitorId);
            guard.AgainstNull(visitor, InvalidVisitorId);

            var skip = (page - 1) * itemsPerPage;
            return visitor!.WantToVisitAttractions
                .Select(fa => new AttractionSimpleViewModel
                {
                    Name = fa.Attraction.Name,
                    CategoryName = fa.Attraction.Category.Name,
                    SubcategoryName = fa.Attraction.Subcategory?.Name,
                    RegionName = fa.Attraction.Region.Name,
                    VillageName = fa.Attraction.Village?.Name,
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

        public async Task<int> GetWanToVisitByVisitorIdCountAsync(string visitorId)
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
            string coordinates, IEnumerable<string> categoryIds,
            int page, int itemsPerPage = 12)
        {
            var points = spatialDataService
                .GetGeometryPointsByStringCoordinates(coordinates.Split(',', StringSplitOptions.RemoveEmptyEntries));

            var startPoint = points[0];

            var result = repo
                .AllAsNoTracking().Include(a => a.Images)
                .Include(a => a.Category).Include(a => a.Subcategory)
                .Include(a => a.Region).Include(a => a.Village)
                .Where(a => !categoryIds.Any() ? true : categoryIds.Contains(a.CategoryId));

            var ids = new HashSet<string>();
            foreach (var point in points)
            {
                ids.UnionWith(result.Where(a => context.GetDistance(point, a.Coordinates) <= 15000).Select(a => a.Id));
            }

            var attractions = await result
                .Where(a => ids.Contains(a.Id))
                .OrderBy(a => a.Id)
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage).ToListAsync();

            return attractions
                .Select(a =>
                {
                    var geo = new GeoCoordinate(a.Coordinates.Y, a.Coordinates.X);

                    return new AttractionByRouteViewModel
                    {
                        Id = a.Id,
                        Name = a.Name,
                        RemoteImageUrl = a.Images.Where(i => i.RemoteImageUrl != null).Select(i => i.RemoteImageUrl).FirstOrDefault()!,
                        BlobStorageUrl = a.Images.Where(i => i.BlobStorageUrl != null).Select(i => i.BlobStorageUrl).FirstOrDefault()!,
                        DistanceFromRoad = points.Min(p => geo.GetDistanceTo(new GeoCoordinate(p.Y, p.X))),
                        DistanceFromStartPoint = geo.GetDistanceTo(new GeoCoordinate(startPoint.Y, startPoint.X)),
                        CategoryName = a.Category.Name,
                        VillageName = a.Village?.Name,
                        RegionName = a.Region.Name,
                        SubcategoryName = a.Subcategory?.Name
                    };
                });               
        }

        public async Task<int> GetCountByRouteAndCategoriesAsync(
            string coordinates, IEnumerable<string> categoryIds)
        {
            var points = spatialDataService
                .GetGeometryPointsByStringCoordinates(coordinates.Split(',', StringSplitOptions.RemoveEmptyEntries));

            var result = repo
                .AllAsNoTracking()
                .Where(a => !categoryIds.Any() ? true : categoryIds.Contains(a.CategoryId));

            var ids = new HashSet<string>();
            foreach (var point in points)
            {
                ids.UnionWith(result.Where(a => context.GetDistance(point, a.Coordinates) <= 15000).Select(a => a.Id));
            }

            return await result
                .Where(a => ids.Contains(a.Id))
                .CountAsync();
        }

        public async Task<IEnumerable<AttractionSidebarViewModel>> GetForHomePageAsync()
            => await repo
                .AllAsNoTracking()
                .To<AttractionSidebarViewModel>()
                .Take(5)
                .ToListAsync();

        private IQueryable<Attraction> ApplyFilter(
            string? categoryName = null,
            string? subcategoryName = null,
            string? regionName = null,
            string? villageName = null,
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

            if (string.IsNullOrEmpty(villageName) == false)
            {
                result = result
                    .Where(a => a.Village != null &&
                           a.Village.Name == villageName);
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
    }
}
