using System.Reflection;
using QueryBuilderSpecs.Interfaces;
using QueryBuilderSpecs.Specifications;

namespace QueryBuilderSpecs.QueryBuilder
{
    public abstract class GenericFiltersBuilder<TEntity, TFilter> : IGenericFiltersBuilder<TEntity, TFilter> where TEntity : class where TFilter : class
    {
        private Dictionary<string, Func<TFilter, ISpecification<TEntity>>> _specifications = new();

        public abstract void InitItemSpecifications();

        public GenericFiltersBuilder()
        {
            InitItemSpecifications();
        }

        public void AddSpecification(string filterPropertyName, Func<TFilter, ISpecification<TEntity>> specification)
        {
            _specifications.Add(
                GetFilterNameForProperty(typeof(TFilter).GetProperty(filterPropertyName)),
                specification
            );
        }

        public virtual string GetFilterNameForProperty(PropertyInfo property)
        {
            return $"FilterBy{property.Name}";
        }

        public IQueryable<TEntity> Apply(TFilter filter, IQueryable<TEntity> query)
        {
            var filterParams = filter.GetType().GetProperties();

            ISpecification<TEntity> theSpecificationChain = null;

            foreach (var filterProp in filterParams)
            {
                var paramValue = filterProp.GetValue(filter);
                if (
                    paramValue == null ||
                    !_specifications.TryGetValue(GetFilterNameForProperty(filterProp), out var specificationFunc)
                )
                    continue;

                theSpecificationChain = specificationFunc.Invoke(filter).Next(theSpecificationChain);
            }

            return theSpecificationChain == null ? query : theSpecificationChain.Apply(query);
        }
    }
}
