﻿using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Attractions;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data
{
    public class AttractionsService : IAttractionsService
    {
        private readonly IDeletableEnityRepository<Attraction> repo;

        public AttractionsService(IDeletableEnityRepository<Attraction> repo)
        {
            this.repo = repo;
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(int page, AttractionsFilterModel? filterModel = null, int itemsPerPage = 12)
        {
            var skip = (page - 1) * itemsPerPage;

            var attractions = repo
                .AllAsNoTracking();

            if (string.IsNullOrEmpty(filterModel?.CategoryName) == false)
            {
                attractions = attractions
                    .Where(a => a.Category.Name == filterModel.CategoryName);
            }

            if (string.IsNullOrEmpty(filterModel?.SubcategoryName) == false)
            {
                attractions = attractions
                    .Where(a => a.Subcategory.Name == filterModel.SubcategoryName);
            }

            if (string.IsNullOrEmpty(filterModel?.RegionName) == false)
            {
                attractions = attractions
                    .Where(a => a.Region != null && a.Region.Name == filterModel.RegionName);
            }

            return await attractions
                .To<T>()
                .Skip(skip)
                .Take(itemsPerPage)
                .ToListAsync();          
        }

        public int GetCount(AttractionsFilterModel? filterModel = null)
        {
            if (filterModel == null)
            {
                return this.repo.AllAsNoTracking().Count();
            }

            return this.repo.AllAsNoTracking().Where(a =>
               a.Category.Name == filterModel.CategoryName &&
               a.Subcategory.Name == filterModel.SubcategoryName &&
               a.Region != null && a.Region.Name == filterModel.RegionName).Count();
        }

        public async Task<T?> GetByIdAsync<T>(string id)
            => await repo.AllAsNoTracking()
                 .Where(a => a.Id == id)
                 .To<T>()
                 .FirstOrDefaultAsync();
    }
}
