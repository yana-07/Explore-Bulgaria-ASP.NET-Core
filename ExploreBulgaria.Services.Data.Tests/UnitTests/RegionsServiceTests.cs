using AutoMapper;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Regions;

namespace ExploreBulgaria.Services.Data.Tests.UnitTests
{
    public class RegionsServiceTests : UnitTestsBase
    {
        private IRegionsService regionsService;
        public override void SetUp()
        {
            base.SetUp();

            regionsService = new RegionsService(regionsRepo);

            AutoMapperConfig.MapperInstance = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Region, RegionSelectViewModel>();
                }));
        }

        [Test]
        public async Task GetAllAsync_ShouldWorkCorrectly()
        {
            await SeedDbAsync();

            var regions = await regionsService
                .GetAllAsync<RegionSelectViewModel>();

            Assert.That(regions.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllForCategoryAndSubcategoryAsync_ShouldWorkCorrectly()
        {
            await SeedAttractionsAsync();

            var regions = await regionsService
                .GetAllForCategoryAndSubcategoryAsync<RegionSelectViewModel>(
                "TestCategory", "TestSubcategory");

            Assert.That(regions.Count(), Is.EqualTo(1));
        }
    }
}
