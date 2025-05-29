namespace QueryBuilderSpecs.Interfaces
{
    public interface IGenericFiltersBuilder<TEntity, TFilter> where TEntity : class where TFilter : class
    {
        IQueryable<TEntity> Apply(TFilter filter, IQueryable<TEntity> query);
    }
}
