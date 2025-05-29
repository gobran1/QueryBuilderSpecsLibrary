namespace QueryBuilderSpecs.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity:class;
        Task SaveAsync();

    }
}
