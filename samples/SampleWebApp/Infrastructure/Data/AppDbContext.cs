using Microsoft.EntityFrameworkCore;
using SampleWebApp.Domain.Users;

namespace SampleWebApp.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions <AppDbContext> options) : base(options)
    {
        
    }

    public DbSet<User> Users { get; set; }
}