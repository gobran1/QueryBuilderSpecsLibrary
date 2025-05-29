using QueryBuilderSpecs.samples.SampleWebApp.Domain.Users;
using QueryBuilderSpecs.Specifications;

namespace QueryBuilderSpecs.samples.SampleWebApp.Application.Users.Filters;

public class FilterByEmailSpecification: BaseSpecification<User>,ISpecification<User>
{
    public FilterByEmailSpecification(string? email)
    {
        if (email == null)
            return;
        
         SetCriteria(u => u.Email == email);
    }
}