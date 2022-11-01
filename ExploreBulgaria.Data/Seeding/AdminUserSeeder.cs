using ExploreBulgaria.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ExploreBulgaria.Data.Seeding
{
    public class AdminUserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Users.Any())
            {
                return;
            }

            using (var serviceScope = serviceProvider.CreateScope())
            {
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var user = new ApplicationUser()
                {
                     FirstName = "Admin",
                     LastName = "User",
                     UserName = "admin.user",
                     Email = "adminuser@abv.bg"
                };

                var result = await userManager.CreateAsync(user, "admin@123456");

                await userManager.AddToRoleAsync(user, "Administrator");
            }
        }
    }
}
