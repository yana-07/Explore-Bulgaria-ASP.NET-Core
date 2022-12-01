using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ExploreBulgaria.Data.Seeding
{
    public class SubcategoriesSeeder : ISeeder
    {
        private static string[] categoriesNotAllowed = new[]
           { "село Нисово", "село Турия", "село Ново село (Област Пловдив)", "село Беброво", "село Гаврил Геново", "село Аврен (Област Варна)" };
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Subcategories.Any())
            {
                return;
            }

            using (var serviceScope = serviceProvider.CreateScope())
            {
                var environment = serviceScope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
                var path = environment.WebRootPath + "/attractions/attractions.json";

                var attractionsJson = File.ReadAllText(path);
                var attractionDtos = JsonConvert.DeserializeObject<AttractionDto[]>(attractionsJson)
                   .Where(dto => !string.IsNullOrEmpty(dto.CategoryName) &&
                                 !string.IsNullOrEmpty(dto.SubCategoryName) &&
                                 !categoriesNotAllowed.Contains(dto.CategoryName))
                   .DistinctBy(dto => dto.SubCategoryName)
                   .ToArray();

                await dbContext.Subcategories.AddRangeAsync(GetSubcategories(attractionDtos, dbContext));
                await dbContext.SaveChangesAsync();
            }
        }

        private IEnumerable<Subcategory> GetSubcategories(AttractionDto[] dtos, ApplicationDbContext dbContext)
        {
            foreach (var dto in dtos)
            {
                var category = dbContext.Categories.FirstOrDefaultAsync(c => c.Name == dto.CategoryName).GetAwaiter().GetResult();

                yield return new Subcategory { Name = dto.SubCategoryName, CategoryId = category.Id };
            }
        }
    }
}
