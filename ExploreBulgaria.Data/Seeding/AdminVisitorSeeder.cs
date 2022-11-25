using ExploreBulgaria.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace ExploreBulgaria.Data.Seeding
{
    public class AdminVisitorSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Visitors.Any())
            {
                return;
            }

            using (var serviceScope = serviceProvider.CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var adminUser = await userManager.FindByEmailAsync("adminuser@abv.bg");
                var adminVisitor = new Visitor
                {
                    UserId = adminUser.Id,
                };

                await dbContext.Visitors.AddAsync(adminVisitor);
                await dbContext.SaveChangesAsync();

                await userManager.AddClaimAsync(adminUser, new Claim("urn:exploreBulgaria:visitorId", adminVisitor.Id));
            }
        }
    }
}
