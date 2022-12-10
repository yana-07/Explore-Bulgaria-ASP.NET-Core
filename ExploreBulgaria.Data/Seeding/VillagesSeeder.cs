using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ExploreBulgaria.Data.Seeding
{
    public class VillagesSeeder : ISeeder
    {
        private static string[] categoriesNotAllowed = new[]
           { "село Нисово", "село Турия", "село Ново село (Област Пловдив)", "село Беброво", "село Гаврил Геново", "село Аврен (Област Варна)" };
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Villages.Any())
            {
                return;
            }

            using (var serviceScope = serviceProvider.CreateAsyncScope())
            {
                var environment = serviceScope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
                var path = environment.WebRootPath + "/attractions/attractions.json";

                var attractionsJson = File.ReadAllText(path);
                var attractionDtos = JsonConvert.DeserializeObject<AttractionDto[]>(attractionsJson)
                    .Where(dto => !string.IsNullOrEmpty(dto.CategoryName) &&
                                  !string.IsNullOrEmpty(dto.Location) &&
                                  !categoriesNotAllowed.Contains(dto.CategoryName) &&
                                  dto.AreaName != dto.Location)
                    .DistinctBy(dto => dto.Location)
                    .ToArray();

                await dbContext.Villages.AddRangeAsync(GetLocations(attractionDtos, dbContext));
                await dbContext.SaveChangesAsync();
            }
        }

        private IEnumerable<Village> GetLocations(AttractionDto[] dtos, ApplicationDbContext dbContext)
        {
            foreach (var dto in dtos)
            {
                var region = dbContext.Regions.FirstOrDefaultAsync(r => r.Name == dto.AreaName).GetAwaiter().GetResult();

                yield return new Village { Name = dto.Location, RegionId = region.Id };
            }
        }
    }
}
