using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using QueryBuilderSpecs.Interfaces;

namespace QueryBuilderSpecs.WorkManager
{
    public class UnitOfWork<TDBContext> : IUnitOfWork<TDBContext>, IDisposable where TDBContext : DbContext
    {

        private readonly TDBContext context;
        private readonly IServiceProvider serviceProvider;

        public UnitOfWork(TDBContext context, IServiceProvider serviceProvider)
        {
            this.context = context;
            this.serviceProvider = serviceProvider;
        }

        public IGenericRepository<TDBContext,TEntity> Repository<TEntity>() where TEntity : class
        {
            return serviceProvider.GetRequiredService<IGenericRepository<TDBContext,TEntity>>();
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
