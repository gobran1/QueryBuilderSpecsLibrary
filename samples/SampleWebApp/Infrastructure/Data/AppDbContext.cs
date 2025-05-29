using Microsoft.EntityFrameworkCore;
using QueryBuilderSpecs.samples.SampleWebApp.Domain.Users;

namespace QueryBuilderSpecs.samples.SampleWebApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions <AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
}