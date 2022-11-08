using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace ExploreBulgaria.Data.Seeding
{
    public class AttractionsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Attractions.Any())
            {
                return;
            }

            using (var serviceScope = serviceProvider.CreateScope())
            {
                var environment = serviceScope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
                var path = environment.WebRootPath + "/attractions/attractions.json";

                var attractionsJson = File.ReadAllText(path);
                var attractionDtos = JsonConvert.DeserializeObject<AttractionDto[]>(attractionsJson)
                    .Where(dto => dto.CategoryName != "")
                    .DistinctBy(dto => dto.AttractionName);

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var user = await userManager.FindByEmailAsync("adminuser@abv.bg");//(u => u.Email == "adminuser@abv.bg"); //userManager.FindByEmailAsync("adminuser@abv.bg");

                foreach (var dto in attractionDtos)
                {
                    var category = await dbContext.Categories.FirstOrDefaultAsync(c => c.Name == dto.CategoryName);

                    var subcategory = await dbContext.Subcategories.FirstOrDefaultAsync(sc => sc.Name == dto.SubCategoryName);

                    var region = await dbContext.Regions.FirstOrDefaultAsync(r => r.Name == dto.AreaName); 

                    var images = dto.ImagesUrls
                           .Select(url =>
                           {
                               var idx = url.LastIndexOf('.');
                               return new Image { RemoteImageUrl = url, Extension = url.Substring(idx + 1) };
                           }).ToList();

                    var attraction = new Attraction
                    {
                        Name = dto.AttractionName,
                        CategoryId = category.Id,
                        SubcategoryId = subcategory.Id,
                        Region = region,
                        Location = new NetTopologySuite.Geometries.Point(
                            Convert.ToDouble(dto.Longitude),
                            Convert.ToDouble(dto.Latitude))
                        { SRID = 4326 },
                        Description = dto.Description,
                        CreatedByUserId = user.Id,
                        Images = images
                    };

                    await dbContext.Attractions.AddAsync(attraction);
                }

                await dbContext.SaveChangesAsync();
            }
        }
    }
}
