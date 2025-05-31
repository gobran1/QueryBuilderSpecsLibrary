using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QueryBuilderSpecs.DTO.Pagination;
using QueryBuilderSpecs.Specifications;

namespace QueryBuilderSpecs.Interfaces
{
    public interface IGenericRepository<TDBContext,T> where T : class where TDBContext : DbContext
    {
        public Task<T> ExecuteSingleRawQueryAsync(string rawSql, Func<IQueryable<T>, IQueryable<T>> extendQuery = null);
        public Task<int> ExecuteCountQueryAsync(string rawSql, Func<IQueryable<T>, IQueryable<T>> extendQuery = null);

        public Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>,
            IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "", int take = 0,
            Func<IQueryable<T>, IQueryable<T>> extendQuery = null,
            Expression<Func<T, object>> select = null,
            Expression<Func<T, object>> orderByColumns = null,
            Expression<Func<T, object>> orderByDescColumn = null,
            int? skip = null
        );

        public Func<IQueryable<T>, IQueryable<T>> InjectSpecification<TFilter>(
            IGenericFiltersBuilder<T, TFilter> filterBuilder, TFilter filter, ISpecification<T> specification = null)
            where TFilter : class;

        public Func<IQueryable<T>, IQueryable<T>> InjectSpecification(ISpecification<T> specification);

        public Task<T> GetAsync(Expression<Func<T, bool>> expression, string includeProperties = "",
            Func<IQueryable<T>, IQueryable<T>> extendQuery = null);

        public Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "", Func<IQueryable<T>, IQueryable<T>> extendQuery = null);

        public Task<bool> AnyAsync(Expression<Func<T, bool>> filter = null);


        public Task InsertAsync(T entity);
        public Task InsertRangeAsync(List<T> entity);

        public Task DeleteAsync(object id);

        public Task DeleteRangeAsync(List<T> entities);
        public Task DeleteRangeAsync(Expression<Func<T, bool>> filter);
        public Task DeleteAsync(T entityToDelete);
        public Task UpdateAsync(T entityToUpdate);

        public Task<PagedList<T>> GetPaginatedItemsAsync(PaginationParams paginationParams,
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>,
                IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "", Func<IQueryable<T>, IQueryable<T>> extendQuery = null);
    }
}
