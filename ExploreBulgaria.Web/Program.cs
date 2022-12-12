using Azure.Storage.Blobs;
using ExploreBulgaria.Data;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Data.Seeding;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString, x => x.UseNetTopologySuite()));

builder.Services.AddDefaultIdentity<ApplicationUser>(IdentityOptionsProvider.GetIdentityOptions)
    .AddRoles<ApplicationRole>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddFacebook(fbOptions =>
    {
        fbOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        fbOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    })
    .AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:AppId"];
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:AppSecret"];
    });

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Users/Login";
    options.LogoutPath = "/Users/Logout";
    //options.AccessDeniedPath = "";
});   

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
});

builder.Services.AddControllersWithViews(options =>
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute()));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAntiforgery(options => 
    options.HeaderName = "X-CSRF-TOKEN");

AutoMapperConfig.RegisterMappings(typeof(ErrorViewModel).GetTypeInfo().Assembly);

builder.Services.AddSingleton(AutoMapperConfig.MapperInstance);

builder.Services.AddSingleton(builder.Configuration);

builder.Services.AddSingleton(new BlobServiceClient(builder.Configuration.GetConnectionString("BlobStorageConnection")));

builder.Services.AddResponseCaching();

// Data Services
builder.Services.AddApplicationServices();

var app = builder.Build();

// Seed data on application startup
using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (dbContext.Database.IsSqlServer())
    {
        dbContext.Database.Migrate();
        new ApplicationDbContextSeeder().SeedAsync(dbContext, serviceScope.ServiceProvider).GetAwaiter().GetResult();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePagesWithRedirects("/Home/StatusCodeError?statusCode={0}");
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseStatusCodePagesWithRedirects("/Home/StatusCodeError?statusCode={0}");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseResponseCaching();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areaRoute",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "attractionDetails",
    pattern: "/Attractions/Details/{id}/{information}",
    defaults: new { Controller = "Attractions", Action = "Details" });
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
