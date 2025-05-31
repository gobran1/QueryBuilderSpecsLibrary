using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using QueryBuilderSpecs.DTO.Pagination;
using QueryBuilderSpecs.Extensions;
using QueryBuilderSpecs.Interfaces;
using QueryBuilderSpecs.Specifications;

namespace QueryBuilderSpecs.Repositories
{
    public class GenericRepository<TDBContext,T> : IGenericRepository<TDBContext,T> where T : class where TDBContext : DbContext
    {
        protected readonly TDBContext context;
        protected readonly DbSet<T> dbSet;

        public GenericRepository(TDBContext context)
        {
            this.context = context;
            this.dbSet = context.Set<T>();
        }

        public Func<IQueryable<T>, IQueryable<T>> InjectSpecification<TFilter>(
            IGenericFiltersBuilder<T, TFilter> filterBuilder, TFilter filter, ISpecification<T> specification = null)
            where TFilter : class
        {
            return query =>
            {
                return specification == null
                    ? filterBuilder.Apply(filter, query)
                    : specification.Apply(filterBuilder.Apply(filter, query));
            };
        }

        public Func<IQueryable<T>, IQueryable<T>> InjectSpecification(
            ISpecification<T> specification)
        {
            return specification.Apply;
        }

        public Task<T> ExecuteSingleRawQueryAsync(string rawSql, Func<IQueryable<T>, IQueryable<T>> extendQuery = null)
        {
            var sql = dbSet.FromSqlRaw(rawSql);

            if (extendQuery != null)
                sql = extendQuery(sql);

            return sql.FirstOrDefaultAsync();
        }
        public Task<int> ExecuteCountQueryAsync(string rawSql, Func<IQueryable<T>, IQueryable<T>> extendQuery = null)
        {
            var sql = dbSet.FromSqlRaw(rawSql);

            if (extendQuery != null)
                sql = extendQuery(sql);

            return sql.CountAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "", int take = 0,
            Func<IQueryable<T>, IQueryable<T>> extendQuery = null,
            Expression<Func<T, object>> select = null,
            Expression<Func<T, object>> orderByColumns = null,
            Expression<Func<T, object>> orderByDescColumn = null,
            int? skip = null

        )
        {
            IQueryable<T> query = dbSet;


            if (extendQuery != null)
            {
                query = extendQuery(query);
            }


            if (orderBy != null && !(query is IOrderedQueryable<T>))
            {
                query = orderBy(query);
            }

            if (orderByColumns != null)
            {
                query = (query is IOrderedQueryable<T>) ?
                     ((IOrderedQueryable<T>)(query)).ThenBy(orderByColumns) :
                     query.OrderBy(orderByColumns);
            }


            if (orderByDescColumn != null)
            {
                query = (query is IOrderedQueryable<T>) ?
                     ((IOrderedQueryable<T>)(query)).ThenBy(orderByDescColumn) :
                     query.OrderBy(orderByDescColumn);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (skip != null)
                query = query.Skip((int)skip);



            if (take != 0)
                query = query.Take(take);


            if (select != null)
                query.Select(select);

            return await query.ToListAsync();
        }

        public async Task<int> GetCountAsync(Expression<Func<T, bool>> filter = null, string includeProperties = "", Func<IQueryable<T>, IQueryable<T>> extendQuery = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (extendQuery != null)
            {
                query = extendQuery(query);
            }
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return await query.CountAsync();
        }


        public async Task<bool> AnyAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            return await query.AnyAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression, string includeProperties = "",
            Func<IQueryable<T>, IQueryable<T>> extendQuery = null)
        {
            IQueryable<T> query = dbSet;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (extendQuery != null)
            {
                query = extendQuery(query);
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);

        }

        public async Task InsertAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public async Task DeleteAsync(object id)
        {
            T? entityToDelete = await dbSet.FindAsync(id);

            if (entityToDelete == null)
                throw new ApplicationException($"{nameof(T)} with Id :[{id}] Not Found!");

            dbSet.Remove(entityToDelete);
        }

        public async Task DeleteRangeAsync(List<T> entities)
        {
            foreach (T entity in entities)
            {
                await this.DeleteAsync(entity);
            }
        }

        public async Task DeleteRangeAsync(Expression<Func<T, bool>> filter)
        {
            var entities = await dbSet.Where(filter).ToListAsync();
            if (entities.Count > 0)
            {
                dbSet.RemoveRange(entities);
            }
        }
        public async Task DeleteAsync(T entityToDelete)
        {
            dbSet.Remove(entityToDelete);
        }

        public async Task UpdateAsync(T entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public async Task InsertRangeAsync(List<T> entity)
        {
            await dbSet.AddRangeAsync(entity);
        }

        public Task<PagedList<T>> GetPaginatedItemsAsync(PaginationParams paginationParams,
            Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = "", Func<IQueryable<T>, IQueryable<T>> extendQuery = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (extendQuery != null)
            {
                query = extendQuery(query);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.AsNoTracking().Paginate(paginationParams.PageNumber, paginationParams.PageSize);
        }

    }
}