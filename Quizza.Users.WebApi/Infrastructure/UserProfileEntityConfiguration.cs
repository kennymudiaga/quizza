using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quizza.Users.Domain.Models;

namespace Quizza.Users.WebApi.Infrastructure;

public class UserProfileEntityConfiguration : IEntityTypeConfiguration<UserProfile>
{
    public void Configure(EntityTypeBuilder<UserProfile> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
        builder.Property(x => x.OtherNames).HasMaxLength(50);
        builder.Property(x => x.Email).IsRequired().HasMaxLength(100);
        builder.HasIndex(x => x.Email).IsUnique();
        builder.Property(x => x.Gender).HasMaxLength(6);
        builder.Property(x => x.PasswordToken).HasMaxLength(10);
        builder.Property(x => x.Phone).HasMaxLength(20);
    }
}
