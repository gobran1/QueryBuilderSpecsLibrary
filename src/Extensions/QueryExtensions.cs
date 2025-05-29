using Microsoft.EntityFrameworkCore;
using QueryBuilderSpecs.DTO.Pagination;
using QueryBuilderSpecs.Interfaces;

namespace QueryBuilderSpecs.Extensions
{
    public static class QueryExtensions
    {
        public static async Task<PagedList<T>> Paginate<T>(this IQueryable<T> source, int pageNumber, int pageSize)
        {
            var count = await source.CountAsync();

            var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageSize);
        }


        public static IQueryable<TEntity> ApplyFilter<TEntity, TFilter>(this IQueryable<TEntity> querey, TFilter filter,IGenericFiltersBuilder<TEntity,TFilter> filterBuilder) where TEntity : class where TFilter: class
        {
            return filterBuilder.Apply(filter, querey);
        }
    }
}
