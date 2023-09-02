using Microsoft.EntityFrameworkCore;
using Quizza.Users.Domain.Models;

namespace Quizza.Users.WebApi.Infrastructure;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> dbContextOptions)
        :base(dbContextOptions)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserProfileConfiguration).Assembly);
    }

    public DbSet<UserProfile> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
}
