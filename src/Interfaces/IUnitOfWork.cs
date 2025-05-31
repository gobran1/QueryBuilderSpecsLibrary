using Microsoft.EntityFrameworkCore;

namespace QueryBuilderSpecs.Interfaces
{
    public interface IUnitOfWork<TDBContext> : IDisposable where TDBContext : DbContext
    {
        public IGenericRepository<TDBContext,TEntity> Repository<TEntity>() where TEntity:class;
        Task SaveAsync();
    }
}
