namespace QueryBuilderSpecs.Specifications
{
    public static class SpecificationParser<T> where T : class
    {
        public static IQueryable<T> Parse(IQueryable<T> inputQuery, ISpecification<T> specification)
        {
            var filteredQuery = inputQuery;

            if (specification.Criteria != null)
                filteredQuery = filteredQuery.Where(specification.Criteria);

            if (specification.Includes != null)
                filteredQuery = specification.Includes(filteredQuery);

            if (specification.OrderBy != null)
                filteredQuery = filteredQuery.OrderBy(specification.OrderBy);

            if (specification.OrderByDesc != null)
                filteredQuery = filteredQuery.OrderByDescending(specification.OrderByDesc);



            return filteredQuery;
        }
    }
}
