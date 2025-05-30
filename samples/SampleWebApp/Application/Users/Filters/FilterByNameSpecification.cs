using QueryBuilderSpecs.Specifications;
using SampleWebApp.Domain.Users;

namespace SampleWebApp.Application.Users.Filters;

public class FilterByNameSpecification: BaseSpecification<User>,ISpecification<User>
{
    public FilterByNameSpecification(string? name)
    {
        if (name == null)
            return;
        
         SetCriteria(u => u.Name == name);
    }
}