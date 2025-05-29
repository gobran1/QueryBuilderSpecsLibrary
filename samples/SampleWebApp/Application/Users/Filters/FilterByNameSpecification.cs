using QueryBuilderSpecs.samples.SampleWebApp.Domain.Users;
using QueryBuilderSpecs.Specifications;

namespace QueryBuilderSpecs.samples.SampleWebApp.Application.Users.Filters;

public class FilterByNameSpecification: BaseSpecification<User>,ISpecification<User>
{
    public FilterByNameSpecification(string? name)
    {
        if (name == null)
            return;
        
         SetCriteria(u => u.Name == name);
    }
}