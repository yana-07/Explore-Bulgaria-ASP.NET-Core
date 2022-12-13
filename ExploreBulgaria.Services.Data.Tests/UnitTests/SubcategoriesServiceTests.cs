using AutoMapper;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Regions;
using ExploreBulgaria.Web.ViewModels.Subcategories;

namespace ExploreBulgaria.Services.Data.Tests.UnitTests
{
    public class SubcategoriesServiceTests : UnitTestsBase
    {
        ISubcategoriesService subcategoriesService;

        public override void SetUp()
        {
            base.SetUp();

            subcategoriesService = new SubcategoriesService(subcategoriesRepo);

            AutoMapperConfig.MapperInstance = new Mapper(
                new MapperConfiguration(config =>
                {
                    config.CreateMap<Subcategory, SubcategorySelectViewModel>();
                }));
        }

        [Test]
        public async Task GetAllAsync_ShouldWorkCorrectly()
        {
            await SeedDbAsync();

            var subcategories = await subcategoriesService
                .GetAllAsync<SubcategorySelectViewModel>();

            Assert.That(subcategories.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task GetAllForCategory_ShouldWorkCorrectly()
        {
            await SeedDbAsync();

            var subcategories = await subcategoriesService
                .GetAllForCategory<SubcategorySelectViewModel>("TestCategory");

            Assert.That(subcategories.Count(), Is.EqualTo(1));
        }

    }
}
