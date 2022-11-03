﻿using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Repositories;
using ExploreBulgaria.Services.Data;
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

            return services;
        }
    }
}