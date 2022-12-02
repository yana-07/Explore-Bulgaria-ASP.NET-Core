﻿using ExploreBulgaria.Web.ViewModels.Administration;

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
    }
}