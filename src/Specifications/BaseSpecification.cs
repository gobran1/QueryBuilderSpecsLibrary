using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace QueryBuilderSpecs.Specifications
{
    public abstract class BaseSpecification<T> : ISpecification<T> where T : class
    {

        public ISpecification<T> NextSpecification { get; set; }
        public Expression<Func<T, bool>> Criteria { get; set; }
        public Func<IQueryable<T>, IIncludableQueryable<T, object>> Includes { get; set; }

        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }

        public BaseSpecification()
        {
            
        }

        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public void SetCriteria(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public void SetIncludes(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
        {
            Includes = includes;
        }

        public void SetOrderBy(Expression<Func<T, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        public void SetOrderByDesc(Expression<Func<T, object>> orderByDesc)
        {
            OrderByDesc = orderByDesc;
        }

        public IQueryable<T> Apply(IQueryable<T> query)
        {
            if (NextSpecification == null)
                return SpecificationParser<T>.Parse(query, this);

            return NextSpecification.Apply(SpecificationParser<T>.Parse(query, this));
        }

        public ISpecification<T> Next(ISpecification<T> specification)
        {
            NextSpecification = specification;
            return this;
        }
    }
}
