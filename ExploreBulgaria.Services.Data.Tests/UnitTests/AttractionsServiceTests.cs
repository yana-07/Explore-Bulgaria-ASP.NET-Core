using AutoMapper;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Services.Guards;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Attractions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ExploreBulgaria.Services.Data.Tests.UnitTests
{
    public class AttractionsServiceTests : UnitTestsBase
    {
        private IAttractionsService attractionsService;

        public override void SetUp()
        {
            base.SetUp();

            var guard = new Guard();
            var categoriesServiceMock = new Mock<ICategoriesService>();
            var spatialDataServiceMock = new Mock<ISpatialDataService>();

            attractionsService = new AttractionsService(
                attrRepo, attrTempRepo, guard, categoriesServiceMock.Object,
                spatialDataServiceMock.Object, visitorsRepo, context);

            AutoMapperConfig.MapperInstance = new Mapper(new MapperConfiguration(config =>
            {
                config.CreateMap<Attraction, AttractionSimpleViewModel>();
            }));
        }

        [Test]
        public async Task GetAllAsync_ShouldApplyFilterAndWorkCorrectly()
        {
            await SeedAttractionsAsync();

            var filterModel = new AttractionFilterModel
            {
                 CategoryName = "TestCategory",
                 SubcategoryName = "TestSubcategory",
                 SearchTerm = "TestDescription1",
                 RegionName = "TestRegion",
                 VillageName = "TestVillage"
            };

            var attractions = await attractionsService.GetAllAsync<AttractionSimpleViewModel>(
                1, filterModel, 1);

            Assert.That(attractions.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetCountAsync_ShouldApplyFilterAndWorkCorrectly()
        {
            await SeedAttractionsAsync();

            var filterModel = new AttractionFilterModel
            {
                CategoryName = "TestCategory",
                SubcategoryName = "TestSubcategory",
                SearchTerm = "TestDescription1",
                RegionName = "TestRegion",
                VillageName = "TestVillage"
            };

            var count = await attractionsService.GetCountAsync(filterModel);

            Assert.That(count, Is.EqualTo(1));
        }

        [Test]
        public async Task GetByIdAsync_ShouldWorkCorrectly()
        {
            await SeedAttractionsAsync();

            var dbAttraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            var attraction = await attractionsService
                .GetByIdAsync<AttractionSimpleViewModel>(dbAttraction!.Id);

            Assert.That(attraction.Id, Is.EqualTo(dbAttraction.Id));
            Assert.That(attraction.Name, Is.EqualTo("TestAttraction1"));
        }

        [Test]
        public void GetByIdAsync_ShouldThrowExceptionIfAttractionIdNotValid()
        {
            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                  .GetByIdAsync<AttractionSimpleViewModel>(""));
        }

        [Test]
        public async Task GetByVisitorIdAsync_ShouldWorkCorrectly()
        {
            await SeedAttractionsAsync();

            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            var attractions = (await attractionsService.GetByVisitorIdAsync(
                visitor!.Id, 1, 2)).ToList();

            Assert.That(attractions.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetCountByVisitorIdAsync_ShouldWorkCorrectly()
        {
            await SeedAttractionsAsync();

            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            var count = await attractionsService.GetCountByVisitorIdAsync(visitor!.Id);

            Assert.That(count, Is.EqualTo(2));
        }

        [Test]
        public async Task AddAttractionToFavoritesAsync_ShouldAddTheAttractionIfVisitorHasNotAddedTheAttractionYet()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .All()
                .Include(a => a.AddedToFavoritesByVisitors)
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            await attractionsService.AddAttractionToFavoritesAsync(visitor!.Id, attraction!.Id);

            Assert.True(attraction.AddedToFavoritesByVisitors
                .Any(fa => fa.VisitorId == visitor.Id));
        }

        [Test]
        public async Task AddAttractionToFavoritesAsync_ShouldRemoveTheAttractionIfVisitorHasAlreadyAddedTheAttraction()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .All()
                .Include(a => a.AddedToFavoritesByVisitors)
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            attraction!.AddedToFavoritesByVisitors.Add(new VisitorFavoriteAttraction
            {
                VisitorId = visitor!.Id
            });
            await attrRepo.SaveChangesAsync();

            await attractionsService.AddAttractionToFavoritesAsync(visitor!.Id, attraction!.Id);

            Assert.False(attraction.AddedToFavoritesByVisitors
                .Any(fa => fa.VisitorId == visitor.Id));
        }


        [Test]
        public async Task AddAttractionToFavoritesAsync_ShouldThrowExceptionIfAttractionIdNotValid()
        {
            await SeedAttractionsAsync();

            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                  .AddAttractionToFavoritesAsync(visitor!.Id, ""));
        }

        [Test]
        public async Task AddAttractionToFavoritesAsync_ShouldThrowExceptionIfVisitorIdNotValid()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                  .AddAttractionToFavoritesAsync("", attraction!.Id));
        }

        [Test]
        public async Task AddAttractionToVisitedAsync_ShouldAddTheAttractionIfVisitorHasNotAddedTheAttractionYet()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .All()
                .Include(a => a.VisitedByVisitors)
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            await attractionsService.AddAttractionToVisitedAsync(visitor!.Id, attraction!.Id);

            Assert.True(attraction.VisitedByVisitors
                .Any(va => va.VisitorId == visitor.Id));
        }

        [Test]
        public async Task AddAttractionToVisitedAsync_ShouldRemoveTheAttractionIfVisitorHasAlreadyAddedTheAttraction()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .All()
                .Include(a => a.VisitedByVisitors)
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            attraction!.VisitedByVisitors.Add(new VisitorVisitedAttraction
            {
                VisitorId = visitor!.Id
            });
            await attrRepo.SaveChangesAsync();

            await attractionsService.AddAttractionToVisitedAsync(visitor!.Id, attraction!.Id);

            Assert.False(attraction.VisitedByVisitors
                .Any(va => va.VisitorId == visitor.Id));
        }


        [Test]
        public async Task AddAttractionToVisitedAsync_ShouldThrowExceptionIfAttractionIdNotValid()
        {
            await SeedAttractionsAsync();

            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                  .AddAttractionToVisitedAsync(visitor!.Id, ""));
        }

        [Test]
        public async Task AddAttractionToVisitedAsync_ShouldThrowExceptionIfVisitorIdNotValid()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                  .AddAttractionToVisitedAsync("", attraction!.Id));
        }

        [Test]
        public async Task AddAttractionToVisitedAsync_ShouldRemoveTheAttractionFromWantToVisitCollection()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.WantToVisitAttractions)
                .FirstOrDefaultAsync();

            visitor!.WantToVisitAttractions.Add(new VisitorWantToVisitAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            await attractionsService.AddAttractionToVisitedAsync(visitor.Id, attraction.Id);

            Assert.False(visitor.WantToVisitAttractions
                .Any(wva => wva.AttractionId == attraction.Id));
        }

        [Test]
        public async Task WantToVisitAttractionAsync_ShouldAddTheAttractionIfVisitorHasNotAddedTheAttractionYet()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.WantToVisitAttractions)
                .FirstOrDefaultAsync();

            await attractionsService.WantToVisitAttractionAsync(visitor!.Id, attraction!.Id);

            Assert.True(visitor.WantToVisitAttractions
                .Any(wva => wva.AttractionId == attraction.Id));
        }

        [Test]
        public async Task WantToVisitAttractionAsync_ShouldRemoveTheAttractionIfVisitorHasAlreadyAddedTheAttraction()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.WantToVisitAttractions)
                .FirstOrDefaultAsync();

            visitor!.WantToVisitAttractions.Add(new VisitorWantToVisitAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            await attractionsService.WantToVisitAttractionAsync(visitor!.Id, attraction!.Id);

            Assert.False(visitor.WantToVisitAttractions
                .Any(wva => wva.AttractionId == attraction.Id));
        }


        [Test]
        public async Task WantToVisitAttractionAsync_ShouldThrowExceptionIfAttractionIdNotValid()
        {
            await SeedAttractionsAsync();

            var visitor = await visitorsRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                  .WantToVisitAttractionAsync(visitor!.Id, ""));
        }

        [Test]
        public async Task WantToVisitAttractionAsync_ShouldThrowExceptionIfVisitorIdNotValid()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                  .WantToVisitAttractionAsync("", attraction!.Id));
        }

        [Test]
        public async Task WantToVisitAttractionAsync_ShouldRemoveAttractionFromVisitedCollection()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.VisitedAttractions)
                .FirstOrDefaultAsync();

            visitor!.VisitedAttractions.Add(new VisitorVisitedAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            await attractionsService.WantToVisitAttractionAsync(visitor!.Id, attraction!.Id);

            Assert.False(visitor.VisitedAttractions
                .Any(wva => wva.AttractionId == attraction.Id));
        }

        [Test]
        public async Task IsAddedToFavoritesAsync_ShouldCheckIfAttractionWasAddedToFavorites()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.FavoriteAttractions)
                .FirstOrDefaultAsync();
            visitor!.FavoriteAttractions.Add(new VisitorFavoriteAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            var result = await attractionsService
                .IsAddedToFavoritesAsync(visitor!.Id, attraction!.Id);

            Assert.True(result);
        }

        [Test]
        public async Task IsAddedToFavoritesAsync_ShouldThrowExceptionIfAttractionIdNotValid()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.FavoriteAttractions)
                .FirstOrDefaultAsync();
            visitor!.FavoriteAttractions.Add(new VisitorFavoriteAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                   .IsAddedToFavoritesAsync(visitor.Id, ""));
        }

        [Test]
        public async Task IsAddedToFavoritesAsync_ShouldThrowExceptionIfVisitorIdNotValid()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.FavoriteAttractions)
                .FirstOrDefaultAsync();
            visitor!.FavoriteAttractions.Add(new VisitorFavoriteAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                   .IsAddedToFavoritesAsync("", attraction.Id));
        }

        [Test]
        public async Task IsAddedToVisitedAsync_ShouldCheckIfAttractionWasAddedToVisited()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.VisitedAttractions)
                .FirstOrDefaultAsync();
            visitor!.VisitedAttractions.Add(new VisitorVisitedAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            var result = await attractionsService
                .IsAddedToVisitedAsync(visitor!.Id, attraction!.Id);

            Assert.True(result);
        }

        [Test]
        public async Task IsAddedToVisitedAsync_ShouldThrowExceptionIfAttractionIdNotValid()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.VisitedAttractions)
                .FirstOrDefaultAsync();
            visitor!.VisitedAttractions.Add(new VisitorVisitedAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                   .IsAddedToVisitedAsync(visitor.Id, ""));
        }

        [Test]
        public async Task IsAddedToVisitedAsync_ShouldThrowExceptionIfVisitorIdNotValid()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.VisitedAttractions)
                .FirstOrDefaultAsync();
            visitor!.VisitedAttractions.Add(new VisitorVisitedAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                   .IsAddedToVisitedAsync("", attraction.Id));
        }

        [Test]
        public async Task WantToVisitAsync_ShouldCheckIfVisitorWantsToVisitAttraction()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.WantToVisitAttractions)
                .FirstOrDefaultAsync();
            visitor!.WantToVisitAttractions.Add(new VisitorWantToVisitAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            var result = await attractionsService
                .WantToVisitAsync(visitor!.Id, attraction!.Id);

            Assert.True(result);
        }

        [Test]
        public async Task WantToVisitAsync_ShouldThrowExceptionIfAttractionIdNotValid()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.WantToVisitAttractions)
                .FirstOrDefaultAsync();
            visitor!.WantToVisitAttractions.Add(new VisitorWantToVisitAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                   .WantToVisitAsync(visitor.Id, ""));
        }

        [Test]
        public async Task WantToVisitAsync_ShouldThrowExceptionIfVisitorIdNotValid()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.WantToVisitAttractions)
                .FirstOrDefaultAsync();
            visitor!.WantToVisitAttractions.Add(new VisitorWantToVisitAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                   .WantToVisitAsync("", attraction.Id));
        }

        [Test]
        public async Task GetFavoritesByVisitorIdAsync_ShouldWorkCorrectly()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .Include(a => a.Images)
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.FavoriteAttractions)
                .FirstOrDefaultAsync();
            visitor!.FavoriteAttractions.Add(new VisitorFavoriteAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            var attractions = (await attractionsService.GetFavoritesByVisitorIdAsync(
                visitor.Id, 1, 1)).ToList();

            Assert.That(attractions.Count, Is.EqualTo(1));
            Assert.That(attractions[0].Name, Is.EqualTo(attraction.Name));
        }

        [Test]
        public async Task GetFavoritesByVisitorIdAsync_ShouldThrowExceptionIfVisitorIdNotValid()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.FavoriteAttractions)
                .FirstOrDefaultAsync();
            visitor!.FavoriteAttractions.Add(new VisitorFavoriteAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                  .GetFavoritesByVisitorIdAsync("", 1, 1));
        }

        [Test]
        public async Task GetVisitedByVisitorIdAsync_ShouldWorkCorrectly()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .Include(a => a.Images)
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.VisitedAttractions)
                .FirstOrDefaultAsync();
            visitor!.VisitedAttractions.Add(new VisitorVisitedAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            var attractions = (await attractionsService.GetVisitedByVisitorIdAsync(
                visitor.Id, 1, 1)).ToList();

            Assert.That(attractions.Count, Is.EqualTo(1));
            Assert.That(attractions[0].Name, Is.EqualTo(attraction.Name));
        }

        [Test]
        public async Task GetVisitedByVisitorIdAsync_ShouldThrowExceptionIfVisitorIdNotValid()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.VisitedAttractions)
                .FirstOrDefaultAsync();
            visitor!.VisitedAttractions.Add(new VisitorVisitedAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                  .GetVisitedByVisitorIdAsync("", 1, 1));
        }

        [Test]
        public async Task GetWantToVisitByVisitorIdAsync_ShouldWorkCorrectly()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .Include(a => a.Images)
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.WantToVisitAttractions)
                .FirstOrDefaultAsync();
            visitor!.WantToVisitAttractions.Add(new VisitorWantToVisitAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            var attractions = (await attractionsService.GetWantToVisitByVisitorIdAsync(
                visitor.Id, 1, 1)).ToList();

            Assert.That(attractions.Count, Is.EqualTo(1));
            Assert.That(attractions[0].Name, Is.EqualTo(attraction.Name));
        }

        [Test]
        public async Task GetWantToVisitVisitorIdAsync_ShouldThrowExceptionIfVisitorIdNotValid()
        {
            await SeedAttractionsAsync();

            var attraction = await attrRepo
                .AllAsNoTracking()
                .FirstOrDefaultAsync();
            var visitor = await visitorsRepo
                .All()
                .Include(v => v.WantToVisitAttractions)
                .FirstOrDefaultAsync();
            visitor!.WantToVisitAttractions.Add(new VisitorWantToVisitAttraction
            {
                AttractionId = attraction!.Id
            });
            await visitorsRepo.SaveChangesAsync();

            Assert.ThrowsAsync<ExploreBulgariaException>(
                async () => await attractionsService
                  .GetWantToVisitByVisitorIdAsync("", 1, 1));
        }
    }
}
