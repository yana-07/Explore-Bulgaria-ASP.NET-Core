using Azure.Storage.Blobs;
using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Services.Guards;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Services.Messaging;
using ExploreBulgaria.Web.ViewModels.Administration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System.Text;
using static ExploreBulgaria.Services.Constants.ExceptionConstants;
using static ExploreBulgaria.Services.Constants.MessageConstants;

namespace ExploreBulgaria.Services.Data.Administration
{
    public class AdminService : IAdminService
    {
        private readonly IDeletableEnityRepository<AttractionTemporary> attrTempRepo;
        private readonly IDeletableEnityRepository<Attraction> attrRepo;
        private readonly IDeletableEnityRepository<Category> categoriesRepo;
        private readonly IDeletableEnityRepository<Subcategory> subcategoriesRepo;
        private readonly IDeletableEnityRepository<Region> regionsRepo;
        private readonly IDeletableEnityRepository<Village> villagesRepo;
        private readonly IDeletableEnityRepository<Visitor> visitorsRepo;
        private readonly BlobServiceClient blobServiceClient;
        private readonly ILogger<AdminService> logger;
        private readonly IEmailSender emailSender;
        private readonly IGuard guard;

        public AdminService(
            IDeletableEnityRepository<AttractionTemporary> attrTempRepo,
            IDeletableEnityRepository<Attraction> attrRepo,
            IDeletableEnityRepository<Category> categoriesRepo,
            IDeletableEnityRepository<Subcategory> subcategoriesRepo,
            IDeletableEnityRepository<Region> regionsRepo,
            IDeletableEnityRepository<Village> villagesRepo,
            IDeletableEnityRepository<Visitor> visitorsRepo,
            BlobServiceClient blobServiceClient,
            ILogger<AdminService> logger,
            IEmailSender emailSender,
            IGuard guard)
        {
            this.attrTempRepo = attrTempRepo;
            this.attrRepo = attrRepo;
            this.categoriesRepo = categoriesRepo;
            this.subcategoriesRepo = subcategoriesRepo;
            this.regionsRepo = regionsRepo;
            this.villagesRepo = villagesRepo;
            this.visitorsRepo = visitorsRepo;
            this.blobServiceClient = blobServiceClient;
            this.logger = logger;
            this.emailSender = emailSender;
            this.guard = guard;
        }

        public async Task AddCategoryAsync(string categoryName)
        {
            var exisitingCategory = await categoriesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == categoryName);

            if (exisitingCategory != null)
            {
                throw new ExploreBulgariaException(CategoryAlreadyExists);
            }

            try
            {
                await categoriesRepo.AddAsync(new Category { Name = categoryName });
                await categoriesRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ExploreBulgariaDbException(SavingToDatabase, ex);
            }
        }

        public async Task AddRegionAsync(string regionName)
        {
            var exisitingRegion = await regionsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == regionName);

            if (exisitingRegion != null)
            {
                throw new ExploreBulgariaException(RegionAlreadyExists);
            }

            try
            {
                await regionsRepo.AddAsync(new Region
                {
                    Name = regionName
                });

                await regionsRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ExploreBulgariaDbException(SavingToDatabase, ex);
            }
        }

        public async Task AddSubcategoryAsync(string subcategoryName, string categoryId)
        {
            var category = await categoriesRepo.GetByIdAsync(categoryId);
            guard.AgainstNull(category, InvalidCategoryId);

            var existingSubcategory = await subcategoriesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(sc => sc.Name == subcategoryName);

            if (existingSubcategory != null)
            {
                throw new ExploreBulgariaException(SubcategoryAlreadyExists);
            }

            try
            {
                await subcategoriesRepo.AddAsync(new Subcategory
                {
                    CategoryId = categoryId,
                    Name = subcategoryName
                });

                await subcategoriesRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ExploreBulgariaDbException(SavingToDatabase, ex);
            }
        }

        public async Task AddVillageAsync(string villageName, string regionId)
        {
            var region = await regionsRepo.GetByIdAsync(regionId);
            guard.AgainstNull(region, InvalidRegionId);

            var existingVillage = await villagesRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync(v => v.Name == villageName);

            if (existingVillage != null)
            {
                throw new ExploreBulgariaException(VillageAlreadyExists);
            }

            try
            {
                await villagesRepo.AddAsync(new Village
                {
                    RegionId = regionId,
                    Name = villageName
                });

                await villagesRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ExploreBulgariaDbException(SavingToDatabase, ex);
            }
        }

        public async Task ApproveAsync(AttractionTempDetailsViewModel model)
        {
            var attraction = new Attraction
            {
                CategoryId = model.CategoryId,
                CreatedByVisitorId = model.CreatedByVisitorId,
                Description = model.Description,
                Coordinates = new Point(model.Longitude, model.Latitude)
                { SRID = 4326 },
                Name = model.Name,
                RegionId = model.RegionId,
                SubcategoryId = model.SubcategoryId,
                VillageId = model.VillageId
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

            try
            {
                await attrRepo.AddAsync(attraction);
                await attrRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ExploreBulgariaDbException(SavingToDatabase, ex);
            }

            var attrTempToDelete = await attrTempRepo
                .All().FirstOrDefaultAsync(at => at.Id == model.Id);
            guard.AgainstNull(attrTempToDelete, InvalidAttractionTemporaryId);
            attrTempToDelete!.IsApproved = true;
            attrTempRepo.Delete(attrTempToDelete!);
            await attrTempRepo.SaveChangesAsync();

            var user = await visitorsRepo
                .AllAsNoTracking()
                .Where(v => v.Id == model.CreatedByVisitorId)
                .Select(v => v.User)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(user!.Email) == false)
            {
                try
                {
                    var attachments = new List<EmailAttachment>();
                    var containerClient = blobServiceClient.GetBlobContainerClient("attractions");
                    foreach (var blobName in model.BlobNames.Split(", "))
                    {
                        var blob = blobName.TrimEnd(',');
                        var blobClient = containerClient.GetBlobClient(blob);
                        var result = await blobClient.DownloadAsync();
                        var attachment = new EmailAttachment
                        {
                            fileName = blob,
                            mimeType = result.Value.Details.ContentType
                        };

                        using (var ms = new MemoryStream())
                        {
                            await result.Value.Content.CopyToAsync(ms);
                            attachment.Content = ms.ToArray();
                        }

                        attachments.Add(attachment);
                    }

                    await emailSender.SendEmailAsync(
                        FromEmail, ExploreBgTeam,
                         user.Email, AttractionApprovedSubject,
                         string.Format(AttractionApprovedContent, model.Name),
                         attachments);
                }
                catch (ArgumentException ex)
                {
                    logger.LogError(ex.Message.ToString());
                }
                catch (Exception ex)
                {
                    throw new ExploreBulgariaException(EmailSenderException, ex);
                }
            }
        }

        public async Task<IEnumerable<string>> GetAdminNotifications(string visitorId)
        {
            var visitor = await visitorsRepo.GetByIdAsync(visitorId);
            guard.AgainstNull(visitor, InvalidVisitorId);

            if (!string.IsNullOrEmpty(visitor!.Notifications))
            {
                return visitor!.Notifications.Split(" ");
            }
            else
            {
                return Enumerable.Empty<string>();  
            }
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

        public async Task NotifyAdmin(string groupName)
        {
            var adminVisitor = await visitorsRepo
                .All()
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.User.Email == "adminuser@abv.bg");

            guard.AgainstNull(adminVisitor, InvalidUserId);

            var notifications = adminVisitor!.Notifications;

            if (string.IsNullOrEmpty(notifications) || 
                notifications.Contains(groupName) == false)
            {
                var sb = new StringBuilder(adminVisitor!.Notifications);
                sb.Append($" {groupName}");
                adminVisitor.Notifications = sb.ToString().Trim();
            }

            await visitorsRepo.SaveChangesAsync();
        }

        public async Task RejectAsync(int id, string visitorId)
        {
            var attraction = await attrTempRepo.GetByIdAsync(id);
            guard.AgainstNull(attraction, InvalidAttractionTemporaryId);

            attraction!.IsRejected = true;
            attrTempRepo.Delete(attraction!);

            try
            {
                await attrTempRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ExploreBulgariaDbException(SavingToDatabase, ex);
            }

            var user = await visitorsRepo
                .AllAsNoTracking()
                .Where(v => v.Id == visitorId)
                .Select(v => v.User)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(user!.Email) == false)
            {
                try
                {
                    await emailSender.SendEmailAsync(
                        FromEmail, ExploreBgTeam,
                         user.Email, AttractionRejected,
                         string.Format(AttractionRejectedContent, attraction.Name));
                }
                catch (ArgumentException ex)
                {
                    logger.LogError(ex.Message.ToString());
                }
                catch (Exception ex)
                {
                    throw new ExploreBulgariaException(EmailSenderException, ex);
                }
            }           
        }

        public async Task ClearAdminNotification(string groupName)
        {
            var adminVisitor = await visitorsRepo
                .All()
                .Include(v => v.User)
                .FirstOrDefaultAsync(v => v.User.Email == "adminuser@abv.bg");

            guard.AgainstNull(adminVisitor, InvalidUserId);

            if (string.IsNullOrEmpty(adminVisitor!.Notifications) == false &&
                adminVisitor.Notifications.Contains(groupName))
            {
                adminVisitor.Notifications = adminVisitor
                    .Notifications.Replace(groupName, string.Empty).Trim();

                await visitorsRepo.SaveChangesAsync();
            }
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

        public async Task DeleteAsync(string id)
        {
            var attraction = await attrRepo.GetByIdAsync(id);
            guard.AgainstNull(attraction, InvalidAttractionId);
            attrRepo.Delete(attraction!);
            try
            {
                await attrRepo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ExploreBulgariaDbException(SavingToDatabase, ex);
            }
        }
    }
}
