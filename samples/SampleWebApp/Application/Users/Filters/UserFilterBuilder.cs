using QueryBuilderSpecs.Interfaces;
using QueryBuilderSpecs.QueryBuilder;
using SampleWebApp.Domain.Users;

namespace SampleWebApp.Application.Users.Filters;

public class UserFilterBuilder: GenericFiltersBuilder<User,UserFilter>,IGenericFiltersBuilder<User,UserFilter> 
{
    public override void InitItemSpecifications()
    {
        AddSpecification(
            nameof(UserFilter.Name),
            filter => new FilterByNameSpecification(filter.Name)
        );
        
        AddSpecification(
            nameof(UserFilter.Email),
            filter => new FilterByEmailSpecification(filter.Email)
        );
        
        AddSpecification(
            nameof(UserFilter.IsActive),
            filter => new FilterByIsActiveSpecification(filter.IsActive)
        );
    }
}