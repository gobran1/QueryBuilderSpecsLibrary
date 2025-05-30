using QueryBuilderSpecs.Specifications;
using SampleWebApp.Domain.Users;

namespace SampleWebApp.Application.Users.Filters;

public class FilterByEmailSpecification: BaseSpecification<User>,ISpecification<User>
{
    public FilterByEmailSpecification(string? email)
    {
        if (email == null)
            return;
        
         SetCriteria(u => u.Email == email);
    }
}