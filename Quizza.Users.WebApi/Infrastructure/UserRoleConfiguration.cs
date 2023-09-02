using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizza.Users.Domain.Models;

namespace Quizza.Users.WebApi.Infrastructure
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(r => new { r.UserId, r.Role });
            builder.Property(r => r.Role).IsRequired().HasMaxLength(50);
        }
    }
}
