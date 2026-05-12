using Company.Infrastructure.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.Persistence.Configurations;

/// <summary>
/// قوانین سطح پایگاه داده برای الزامی بودن نام، نام خانوادگی، ایمیل و شماره تماس.
/// </summary>
public sealed class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(128);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(u => u.PhoneNumber)
            .IsRequired()
            .HasMaxLength(32);

        builder.Property(u => u.UserName)
            .IsRequired()
            .HasMaxLength(256);
    }
}
