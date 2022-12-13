using ExploreBulgaria.Data.Common.Repositories;
using ExploreBulgaria.Data.Models;
using ExploreBulgaria.Services.Exceptions;
using ExploreBulgaria.Services.Guards;
using static ExploreBulgaria.Services.Constants.ExceptionConstants;

namespace ExploreBulgaria.Services.Data
{
    public class VisitorsService : IVisitorsService
    {
        private readonly IDeletableEnityRepository<Visitor> repo;
        private readonly IGuard guard;

        public VisitorsService(
            IDeletableEnityRepository<Visitor> repo,
            IGuard guard)
        {
            this.repo = repo;
            this.guard = guard;
        }    

        public async Task<string> CreateByUserId(string userId)
        {
            var visitor = new Visitor
            {
                UserId = userId
            };

            try
            {
                await repo.AddAsync(visitor);
                await repo.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new ExploreBulgariaDbException(SavingToDatabase, ex);
            }

            return visitor.Id;
        }        
    }
}
