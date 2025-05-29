using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace QueryBuilderSpecs.Specifications
{
    public interface ISpecification<T> where T : class
    {
        public ISpecification<T> NextSpecification { get; set; }

        public Expression<Func<T, bool>> Criteria { get; set; }

        public Expression<Func<T, object>> OrderBy { get; set; }

        public Expression<Func<T, object>> OrderByDesc { get; set; }

        public Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes { get; set; }

        public IQueryable<T> Apply(IQueryable<T> query);

        public ISpecification<T> Next(ISpecification<T> specification);
    }
}
