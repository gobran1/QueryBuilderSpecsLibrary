using Microsoft.EntityFrameworkCore;
using QueryBuilderSpecs.Interfaces;

namespace QueryBuilderSpecs.WorkManager
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private readonly DbContext context;
        private readonly IServiceProvider serviceProvider;

        public UnitOfWork(DbContext context, IServiceProvider serviceProvider)
        {
            this.context = context;
            this.serviceProvider = serviceProvider;
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return serviceProvider.GetRequiredService<IGenericRepository<TEntity>>();
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

    }
}
