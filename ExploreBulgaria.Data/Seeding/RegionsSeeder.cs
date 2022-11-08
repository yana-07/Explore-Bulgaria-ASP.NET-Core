using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ExploreBulgaria.Data.Seeding
{
    public class RegionsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Regions.Any())
            {
                return;
            }

            using (var serviceScope = serviceProvider.CreateScope())
            {
                var environment = serviceScope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
                var path = environment.WebRootPath + "/attractions/attractions.json";

                var attractionsJson = File.ReadAllText(path);
                var attractionDtos = JsonConvert.DeserializeObject<AttractionDto[]>(attractionsJson)
                    .Where(x => x.CategoryName != "");

                var regions = attractionDtos
                    .DistinctBy(dto => dto.AreaName)
                    .Select(dto => new Region { Name = dto.AreaName });

                await dbContext.Regions.AddRangeAsync(regions);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
