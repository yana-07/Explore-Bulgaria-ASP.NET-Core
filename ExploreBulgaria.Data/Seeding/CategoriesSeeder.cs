using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ExploreBulgaria.Data.Seeding
{
    public class CategoriesSeeder : ISeeder
    {
        private static string[] categoriesNotAllowed = new[] 
           { "село Нисово", "село Турия", "село Ново село (Област Пловдив)", "село Беброво", "село Гаврил Геново", "село Аврен (Област Варна)" };
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Categories.Any())
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
                           !categoriesNotAllowed.Contains(dto.CategoryName));

                var categories = attractionDtos
                    .DistinctBy(dto => dto.CategoryName)
                    .Select(dto => new Category { Name = dto.CategoryName });

                await dbContext.Categories.AddRangeAsync(categories);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
