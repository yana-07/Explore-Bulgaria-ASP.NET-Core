using ExploreBulgaria.Web.ViewModels.Administration;

namespace ExploreBulgaria.Services.Data.Administration
{
    public interface IAdminService
    {
        Task<IEnumerable<T>> GetAllAsync<T>(
           int page,
           AttractionTemporaryFilterModel filterModel,
           int itemsPerPage);

        Task<T> GetByIdAsync<T>(int id);

        Task<int> GetCountAsync(AttractionTemporaryFilterModel filterModel);

        Task ApproveAsync(AttractionTempDetailsViewModel model);

        Task RejectAsync(int id, string visitorId);

        Task AddCategoryAsync(string categoryName);

        Task AddSubcategoryAsync(string subcategoryName, string categoryId);

        Task AddRegionAsync(string regionName);

        Task AddVillageAsync(string villageName, string regionId);

        Task NotifyAdmin(string groupName);

        Task ClearAdminNotification(string groupName);

        Task<IEnumerable<string>> GetAdminNotifications(string visitorId);

        Task DeleteAsync(string id);

        Task<T> GetForEdit<T>(string id);

        Task SaveEdited(EditAttractionViewModel model);
    }
}
