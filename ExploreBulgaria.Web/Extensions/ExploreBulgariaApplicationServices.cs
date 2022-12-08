using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Repositories;
using ExploreBulgaria.Services.Guards;
using ExploreBulgaria.Services.Data;
using ExploreBulgaria.Services.Data.Administration;
using ExploreBulgaria.Web.ViewModels.Users;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ExploreBulgariaApplicationServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(IDeletableEnityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IAttractionsService, AttractionsService>();
            services.AddScoped<ITemporaryAttractionsService, TemporaryAttractionsService>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<ISubcategoriesService, SubcategoriesService>();
            services.AddScoped<IRegionsService, RegionsService>();
            services.AddScoped<ILocationsService, LocationsService>();
            services.AddScoped<ICommentsService, CommentsService>();
            services.AddScoped<IVisitorsService, VisitorsService>();
            services.AddScoped<IVotesService, VotesService>();
            services.AddScoped<IVisitorsService, VisitorsService>();
            services.AddScoped<IGuard, Guard>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<ISpatialDataService, SpatialDataService>();

            return services;
        }
    }
}
