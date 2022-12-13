using AutoMapper;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Villages;

namespace ExploreBulgaria.Services.Data.Tests.UnitTests
{
    public class VillagesServiceTests : UnitTestsBase
    {
        IVillagesService villagesService;

        public override void SetUp()
        {
            base.SetUp();

            villagesService = new VillagesService(villagesRepo);

            AutoMapperConfig.MapperInstance = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Village, VillageSelectViewModel>();
                }));
        }

        [Test]
        public async Task GetAllAsync_ShouldWorkCorrectly()
        {
            await SeedDbAsync();

            var villages = (await villagesService
                .GetAllAsync<VillageSelectViewModel>())
                .ToList();

            Assert.That(villages.Count(), Is.EqualTo(1));
            Assert.That(villages[0].Name, Is.EqualTo("TestVillage"));
        }

        [Test]
        public async Task GetAllForCategorySubcategoryAndRegionAsync_ShouldWorkCorrectly()
        {
            await SeedAttractionsAsync();

            var villages = await villagesService
                .GetAllForCategorySubcategoryAndRegionAsync<VillageSelectViewModel>(
                "TestCategory", "TestSubcategory", "TestRegion");

            Assert.That(villages.Count(), Is.EqualTo(1));
        }
    }
}
