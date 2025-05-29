using QueryBuilderSpecs.samples.SampleWebApp.Domain.Users;
using QueryBuilderSpecs.Specifications;

namespace QueryBuilderSpecs.samples.SampleWebApp.Application.Users.Filters;

public class FilterByIsActiveSpecification: BaseSpecification<User>,ISpecification<User>
{
    public FilterByIsActiveSpecification(bool? isActive)
    {
        if (isActive == null)
            return;
        
         SetCriteria(u => u.IsActive == isActive);
    }
}