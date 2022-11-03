using ExploreBulgaria.Data;
using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Data.Repositories;
using ExploreBulgaria.Data.Seeding;
using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Web.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, x => x.UseNetTopologySuite()));

builder.Services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
    .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton(builder.Configuration);

// Data Repositories
builder.Services.AddApplicationServices();

var app = builder.Build();

// Seed data on application startup
using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //dbContext.Database.Migrate();
    new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
