using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizza.Users.Domain.Models.Entities;

namespace Quizza.Users.Application.Infrastructure
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.HasKey(r => new { r.UserProfileId, r.Role });
            builder.Property(r => r.Role).IsRequired().HasMaxLength(50);
        }
    }
}
