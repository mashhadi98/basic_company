using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(p => p.Id);

        // General Information
        builder.Property(p => p.Title)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(p => p.Slug)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(p => p.ShortDescription)
            .HasMaxLength(1000);

        builder.Property(p => p.FullDescription)
            .HasColumnType("NVARCHAR(MAX)");

        builder.Property(p => p.MainImage)
            .HasMaxLength(512);

        builder.Property(p => p.IsFeatured)
            .HasDefaultValue(false);

        builder.Property(p => p.PublishStatus)
            .HasDefaultValue(PublishStatus.Draft);

        builder.Property(p => p.SortOrder)
            .HasDefaultValue(0);

        // SEO Fields
        builder.Property(p => p.SeoMetaTitle)
            .HasMaxLength(160);

        builder.Property(p => p.SeoMetaDescription)
            .HasMaxLength(160);

        builder.Property(p => p.SeoKeywords)
            .HasMaxLength(500);

        builder.Property(p => p.CanonicalUrl)
            .HasMaxLength(512);

        // Media & Files
        builder.Property(p => p.ThumbnailImage)
            .HasMaxLength(512);

        builder.Property(p => p.CatalogPdfFile)
            .HasMaxLength(512);

        builder.Property(p => p.VideoUrl)
            .HasMaxLength(512);

        // Audit Information
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(p => p.CreatedBy)
            .HasMaxLength(256);

        builder.Property(p => p.UpdatedBy)
            .HasMaxLength(256);

        // Foreign Key
        builder.HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        // Navigation Properties
        builder.HasMany(p => p.Attributes)
            .WithOne(a => a.Product)
            .HasForeignKey(a => a.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.GalleryImages)
            .WithOne(g => g.Product)
            .HasForeignKey(g => g.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Tags)
            .WithOne(t => t.Product)
            .HasForeignKey(t => t.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(p => p.Slug).IsUnique();
        builder.HasIndex(p => p.PublishStatus);
        builder.HasIndex(p => p.SortOrder);
        builder.HasIndex(p => p.CreatedAt);
    }
}
