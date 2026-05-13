using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.Persistence.Configurations;

public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
{
    public void Configure(EntityTypeBuilder<ProductCategory> builder)
    {
        builder.ToTable("ProductCategories");

        builder.HasKey(pc => pc.Id);

        builder.Property(pc => pc.Title)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(pc => pc.Slug)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(pc => pc.Description)
            .HasMaxLength(1000);

        builder.Property(pc => pc.Image)
            .HasMaxLength(512);

        builder.Property(pc => pc.SortOrder)
            .HasDefaultValue(0);

        builder.Property(pc => pc.IsPublished)
            .HasDefaultValue(false);

        builder.Property(pc => pc.SeoMetaTitle)
            .HasMaxLength(160);

        builder.Property(pc => pc.SeoMetaDescription)
            .HasMaxLength(160);

        builder.Property(pc => pc.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        // Self-referencing foreign key for hierarchical categories
        builder.HasOne(pc => pc.ParentCategory)
            .WithMany(pc => pc.ChildCategories)
            .HasForeignKey(pc => pc.ParentCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(pc => pc.Slug).IsUnique();
        builder.HasIndex(pc => pc.IsPublished);
        builder.HasIndex(pc => pc.SortOrder);
    }
}
