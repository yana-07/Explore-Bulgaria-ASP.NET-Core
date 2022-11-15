using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Mapping;
using ExploreBulgaria.Web.ViewModels.Visitors;
using Microsoft.EntityFrameworkCore;

namespace ExploreBulgaria.Services.Data
{
    public class VisitorsService : IVisitorsService
    {
        private readonly IDeletableEnityRepository<Visitor> repo;

        public VisitorsService(IDeletableEnityRepository<Visitor> repo)
        {
            this.repo = repo;
        }

        public async Task<string> CreateByUserId(string userId)
        {
            var visitor = new Visitor
            {
                UserId = userId
            };

            await repo.AddAsync(visitor);

            await repo.SaveChangesAsync();

            return visitor.Id;
        }
    }
}
