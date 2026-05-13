using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.Persistence.Configurations;

public class StaticPageConfiguration : IEntityTypeConfiguration<StaticPage>
{
    public void Configure(EntityTypeBuilder<StaticPage> builder)
    {
        builder.ToTable("StaticPages");

        builder.HasKey(sp => sp.Id);

        builder.Property(sp => sp.Key)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(sp => sp.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(sp => sp.Summary)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(sp => sp.Description)
            .IsRequired(); // بدون محدودیت طول برای توضیحات کامل

        builder.Property(sp => sp.Image)
            .HasMaxLength(500);

        builder.Property(sp => sp.IsPublished)
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(sp => sp.Key)
            .IsUnique(); // کلید باید منحصر به فرد باشد

        builder.HasIndex(sp => sp.IsPublished);
    }
}