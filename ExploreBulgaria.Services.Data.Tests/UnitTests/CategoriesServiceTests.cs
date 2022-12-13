using AutoMapper;
using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Data.Repositories;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Services.Guards;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Categories;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data.Tests.UnitTests
{
    public class CategoriesServiceTests : UnitTestsBase
    {
        private ICategoriesService categoriesService;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            categoriesService = new CategoriesService(categoriesRepo, guard);

            AutoMapperConfig.MapperInstance = new Mapper(new MapperConfiguration(config =>
            {
                config.CreateMap<Category, CategoryOptionViewModel>();
            }));
        }

        [Test]
        public async Task GetAllAsync_ShouldWorkCorrectly()
        {
            await SeedDbAsync(categoriesRepo);

            var categories = (await categoriesService
                .GetAllAsync<CategoryOptionViewModel>()).ToList();

            Assert.That(categories.Count(), Is.EqualTo(2));
            Assert.That(categories[0].Name, Is.EqualTo("TestCategory1"));
            Assert.That(categories[1].Name, Is.EqualTo("TestCategory2"));
        }

        [Test]
        public async Task GetAllForRegionAsync_ShouldWorkCorrectly()
        {
            await SeedDbAsync(categoriesRepo);

            var categories = (await categoriesService
                .GetAllForRegionAsync<CategoryOptionViewModel>("Test2"))
                .ToList();

            Assert.That(categories[0].Name, Is.EqualTo("TestCategory2"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldWorkCorrectly()
        {
            await SeedDbAsync(categoriesRepo);

            var dbFirstCategoryId = (await categoriesRepo.AllAsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == "TestCategory1"))!
                .Id;
            var dbSecondCategoryId = (await categoriesRepo.AllAsNoTracking()
                .FirstOrDefaultAsync(c => c.Name == "TestCategory2"))!
                .Id;
            var firstCategory = await categoriesService
                .GetByIdAsync<CategoryOptionViewModel>(dbFirstCategoryId);
            var secondCategory = await categoriesService
                .GetByIdAsync<CategoryOptionViewModel>(dbSecondCategoryId);

            Assert.That(firstCategory.Name, Is.EqualTo("TestCategory1"));
            Assert.That(secondCategory.Name, Is.EqualTo("TestCategory2"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldThrowExceptionIfCategoryIdNotValid()
        {
            await SeedDbAsync(categoriesRepo);

            Assert.ThrowsAsync<ExploreBulgariaException>(async () => await categoriesService
               .GetByIdAsync<CategoryOptionViewModel>(""));
        }

        private async Task SeedDbAsync(IDeletableEnityRepository<Category> repo)
        {
            await repo.AddAsync(new Category
            {
                Name = "TestCategory1",
                Attractions = new List<Attraction>
                {
                    new Attraction 
                    { 
                        Name = "1",
                        CreatedByVisitor = new Visitor 
                        {
                            User = new ApplicationUser 
                            {
                                 UserName = "TestUserName1",
                                 Email = "test1@mail.com"
                            } 
                        },
                        Description = "",                       
                        Region = new Region { Name = "Test1" }
                    }
                }
            }); 
            await repo.AddAsync(new Category
            {
                Name = "TestCategory2",
                Attractions = new List<Attraction>
                {
                    new Attraction
                    {
                        Name = "2",
                        CreatedByVisitor = new Visitor
                        {
                            User = new ApplicationUser
                            {
                                 UserName = "TestUserName2",
                                 Email = "test2@mail.com"
                            }
                        },
                        Description = "",
                        Region = new Region { Name = "Test2" }
                    }
                }
            });

            await repo.SaveChangesAsync();
        }
    }
}
