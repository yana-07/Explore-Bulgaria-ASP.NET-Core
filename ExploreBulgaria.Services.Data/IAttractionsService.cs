using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Enums;
using ExploreBulgaria.Web.ViewModels.Attractions;
using NetTopologySuite.Geometries;
using System.Collections.Generic;

namespace ExploreBulgaria.Services.Data
{
    public interface IAttractionsService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(
            int page,
            AttractionFilterModel filterModel,
            int itemsPerPage = 12);

        Task<int> GetCountAsync(AttractionFilterModel filterModel);

        Task<T> GetByIdAsync<T>(string id);

        Task<IEnumerable<AttractionSimpleViewModel>> GetByVisitorIdAsync(
            string visitorId, int page, int itemsPerPage = 12);

        Task<int> GetCountByVisitorIdAsync(string visitorId);

        Task AddAttractionToFavoritesAsync(string visitorId, string attractionId);

        Task WantToVisitAttractionAsync(string visitorId, string attractionId);

        Task AddAttractionToVisitedAsync(string visitorId, string attractionId);

        Task<bool> IsAddedToFavoritesAsync(string visitorId, string attractionId);

        Task<bool> IsAddedToVisitedAsync(string visitorId, string attractionId);

        Task<bool> WantToVisitAsync(string visitorId, string attractionId);

        Task<IEnumerable<AttractionSimpleViewModel>> GetFavoritesByVisitorIdAsync(
            string visitorId, int page, int itemsPerPage = 12);

        Task<IEnumerable<AttractionSimpleViewModel>> GetVisitedByVisitorIdAsync(
            string visitorId, int page, int itemsPerPage = 12);

        Task<IEnumerable<AttractionSimpleViewModel>> GetWantToVisitByVisitorIdAsync(
            string visitorId, int page, int itemsPerPage = 12);

        Task<int> GetFavoritesByVisitorIdCountAsync(string visitorId);

        Task<int> GetVisitedByVisitorIdCountAsync(string visitorId);

        Task<int> GetWanToVisitByVisitorIdCount(string visitorId);

        Task<IEnumerable<AttractionSidebarViewModel>> GetSidebarAttractions(SidebarOrderEnum orderBy);

        Task<IEnumerable<T>> GetByCategories<T>(params string[] categoryIds);

        Task<IEnumerable<AttractionByRouteViewModel>> GetByRouteAndCategoriesAsync(
            string coordinates, IEnumerable<string> categoryIds, int page, int itemsPerPage = 12);

        Task<int> GetCountByRouteAndCategoriesAsync(string coordinates, IEnumerable<string> categoryIds);
    }
}
