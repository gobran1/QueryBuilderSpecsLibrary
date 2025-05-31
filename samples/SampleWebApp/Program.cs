using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueryBuilderSpecs.Extensions;
using QueryBuilderSpecs.Interfaces;
using SampleWebApp.Application.Users.Filters;
using SampleWebApp.Domain.Users;
using SampleWebApp.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

// Register QueryBuilderSpecs services
builder.Services.UseQueryBuilderSpecs();
builder.Services.AddFilterBuilder<User, UserFilter, UserFilterBuilder>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Minimal API Endpoints

app.MapGet("/api/users/from-dbcontext", GetUsersFromDbContext)
    .WithName("GetUsersUsingDbContext")
    .WithOpenApi();

app.MapGet("/api/users/from-uow", GetUsersFromUnitOfWork)
    .WithName("GetUsersUsingUnitOfWork")
    .WithOpenApi();

// Seed demo data
await SeedUsersAsync(app.Services);

app.Run();

return;

// --- Endpoint Handlers ---

static async Task<IResult> GetUsersFromDbContext(
    [AsParameters] UserFilter userFilter,
    [FromServices] AppDbContext dbContext,
    [FromServices] IGenericFiltersBuilder<User, UserFilter> userFilterBuilder)
{
    var users = await dbContext.Users
        .ApplyFilter(userFilter, userFilterBuilder)
        .ToListAsync();

    return Results.Ok(new { users });
}

static async Task<IResult> GetUsersFromUnitOfWork(
    [AsParameters]UserFilter userFilter,
    [FromServices] IUnitOfWork<AppDbContext> unitOfWork,
    [FromServices] IGenericFiltersBuilder<User, UserFilter> userFilterBuilder)
{
    var specification = unitOfWork.Repository<User>()
        .InjectSpecification(userFilterBuilder, userFilter);

    var users = await unitOfWork.Repository<User>()
        .GetAllAsync(extendQuery: specification);

    return Results.Ok(new { users });
}

// --- Seeding Function ---

static async Task SeedUsersAsync(IServiceProvider services)
{
    using var scope = services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (await dbContext.Users.AnyAsync()) return;

    var users = new List<User>
    {
        new() { Id = 1, Name = "John Doe", Email = "johndoe@test.com", IsActive = true },
        new() { Id = 2, Name = "Test User", Email = "test@test.com", IsActive = false },
    };

    await dbContext.Users.AddRangeAsync(users);
    await dbContext.SaveChangesAsync();
}