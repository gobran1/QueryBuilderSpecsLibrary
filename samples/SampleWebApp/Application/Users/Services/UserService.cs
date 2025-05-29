using Microsoft.EntityFrameworkCore;
using QueryBuilderSpecs.Extensions;
using QueryBuilderSpecs.Interfaces;
using QueryBuilderSpecs.samples.SampleWebApp.Application.Users.Filters;
using QueryBuilderSpecs.samples.SampleWebApp.Domain.Users;
using QueryBuilderSpecs.samples.SampleWebApp.Infrastructure.Data;

namespace QueryBuilderSpecs.samples.SampleWebApp.Application.Users.Services;

public class UserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly AppDbContext _context;
    private readonly IGenericFiltersBuilder<User, UserFilter> _userFilterBuilder;

    public UserService(IUnitOfWork  unitOfWork,AppDbContext context,IGenericFiltersBuilder<User,UserFilter> userFilterBuilder)
    {
        _unitOfWork = unitOfWork;
        _context = context;
        _userFilterBuilder = userFilterBuilder;
    }
    
    public async Task<List<User>> GetAllUsingUnitOfWork(UserFilter userFilter)
    {

        var injectedFilter = _unitOfWork.Repository<User>().InjectSpecification(_userFilterBuilder, userFilter);
        
        return await _unitOfWork.Repository<User>().GetAllAsync(
            extendQuery:injectedFilter
        );
    }
    
    public Task<List<User>> GetAllUsingDbContext(UserFilter userFilter)
    {
        return _context.Users.ApplyFilter(userFilter, _userFilterBuilder).ToListAsync();
    }
}