using ExploreBulgaria.Data.Common.Models;

namespace ExploreBulgaria.Data.Common.Repositories
{
    public interface IDeletableEnityRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IDeletableEntity
    {
        IQueryable<TEntity> AllWithDeleted();

        IQueryable<TEntity> AllAsNoTrackingWithDeleted();

        void HardDelete(TEntity entity);

        void Undelete(TEntity entity);
    }
}
