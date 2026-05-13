using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.Persistence.Configurations;

public class CompanyFeatureConfiguration : IEntityTypeConfiguration<CompanyFeature>
{
    public void Configure(EntityTypeBuilder<CompanyFeature> builder)
    {
        builder.ToTable("CompanyFeatures");

        builder.HasKey(cf => cf.Id);

        builder.Property(cf => cf.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(cf => cf.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(cf => cf.Icon)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(cf => cf.SortOrder)
            .HasDefaultValue(0);

        builder.Property(cf => cf.IsPublished)
            .HasDefaultValue(false);

        // Indexes
        builder.HasIndex(cf => cf.SortOrder);
        builder.HasIndex(cf => cf.IsPublished);
    }
}