using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.Persistence.Configurations;

public class SiteSettingConfiguration : IEntityTypeConfiguration<SiteSetting>
{
    public void Configure(EntityTypeBuilder<SiteSetting> builder)
    {
        builder.ToTable("SiteSettings");

        builder.HasKey(ss => ss.Id);

        builder.Property(ss => ss.Key)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(ss => ss.Value)
            .HasMaxLength(5000)
            .IsRequired();

        builder.Property(ss => ss.Type)
            .HasDefaultValue(SiteSettingType.String);

        builder.Property(ss => ss.SortOrder)
            .HasDefaultValue(0);

        builder.Property(ss => ss.IsPublished)
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(ss => ss.Key).IsUnique();
        builder.HasIndex(ss => ss.SortOrder);
        builder.HasIndex(ss => ss.IsPublished);
    }
}
