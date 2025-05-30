using QueryBuilderSpecs.Specifications;
using SampleWebApp.Domain.Users;

namespace SampleWebApp.Application.Users.Filters;

public class FilterByIsActiveSpecification: BaseSpecification<User>,ISpecification<User>
{
    public FilterByIsActiveSpecification(bool? isActive)
    {
        if (isActive == null)
            return;
        
         SetCriteria(u => u.IsActive == isActive);
    }
}