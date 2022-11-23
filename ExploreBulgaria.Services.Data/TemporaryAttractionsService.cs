using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Common.Guards;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Administration;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using static ExploreBulgaria.Services.Common.Constants.ExceptionConstants;

namespace ExploreBulgaria.Services.Data
{
    public class TemporaryAttractionsService : ITemporaryAttractionsService
    {
        private readonly IDeletableEnityRepository<AttractionTemporary> attrTempRepo;
        private readonly IDeletableEnityRepository<Attraction> attrRepo;
        private readonly IGuard guard;

        public TemporaryAttractionsService(
            IDeletableEnityRepository<AttractionTemporary> attrTempRepo,
            IDeletableEnityRepository<Attraction> attrRepo,
            IGuard guard)
        {
            this.attrTempRepo = attrTempRepo;
            this.attrRepo = attrRepo;
            this.guard = guard;
        }

        public async Task ApproveAsync(AttractionTempDetailsViewModel model)
        {
            var attraction = new Attraction
            {
                CategoryId = model.CategoryId,
                CreatedByVisitorId = model.CreatedByVisitorId,
                Description = model.Description,
                Location = new Point(model.Longitude, model.Latitude)
                { SRID = 4326 },
                Name = model.Name,
                RegionId = model.RegionId,
                SubcategoryId = model.SubcategoryId,
            };

            foreach (var blob in model.BlobNames.Split(", "))
            {
                attraction.Images.Add(new Image
                {
                     AddedByVisitorId = model.CreatedByVisitorId,
                     BlobStorageUrl = blob.TrimEnd(','),
                     Extension = Path.GetExtension(blob).Trim('.', ',')
                });
            }

            var attrTempToDelete = await attrTempRepo
                .All().FirstOrDefaultAsync(at => at.Id == model.Id);

            await attrRepo.AddAsync(attraction);
            await attrRepo.SaveChangesAsync();

            attrTempRepo.Delete(attrTempToDelete!);
            await attrTempRepo.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync<T>(int page,
            AttractionTemporaryFilterModel filterModel,
            int itemsPerPage)
        {
            var skip = (page - 1) * itemsPerPage;

            var attractionsTemp = ApplyFilter(filterModel);

            return await attractionsTemp
                .Skip(skip)
                .Take(itemsPerPage)
                .To<T>()
                .ToListAsync();
        }

        public async Task<T> GetByIdAsync<T>(int id)
        {
            var attraction = await attrTempRepo
                .AllAsNoTracking()
                .Where(at => at.Id == id)
                .To<T>()
                .FirstOrDefaultAsync();

            guard.AgainstNull(attraction, InvalidAttractionTemporaryId);

            return attraction!;
        }

        public async Task<int> GetCountAsync(AttractionTemporaryFilterModel filterModel)
        {
            var attractionsTemp = ApplyFilter(filterModel);

            return await attractionsTemp.CountAsync();
        }

        private IQueryable<AttractionTemporary> ApplyFilter(AttractionTemporaryFilterModel filterModel)
        {
            var result = attrTempRepo.AllAsNoTracking();

            var searchTerm = filterModel.SearchTerm;

            if (string.IsNullOrEmpty(searchTerm) == false)
            {
                searchTerm = $"%{searchTerm.ToLower()}%";

                result = result.Where(at => EF.Functions.Like(at.Name.ToLower(), searchTerm) ||
                        EF.Functions.Like(at.Description.ToLower(), searchTerm));
            }

            return result;
        }
    }
}
