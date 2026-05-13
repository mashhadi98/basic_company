using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.Persistence.Configurations;

public class ProductGalleryConfiguration : IEntityTypeConfiguration<ProductGallery>
{
    public void Configure(EntityTypeBuilder<ProductGallery> builder)
    {
        builder.ToTable("ProductGalleries");

        builder.HasKey(pg => pg.Id);

        builder.Property(pg => pg.ImageUrl)
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(pg => pg.AltText)
            .HasMaxLength(512);

        builder.Property(pg => pg.SortOrder)
            .HasDefaultValue(0);

        builder.Property(pg => pg.IsPrimary)
            .HasDefaultValue(false);

        // Foreign Key
        builder.HasOne(pg => pg.Product)
            .WithMany(p => p.GalleryImages)
            .HasForeignKey(pg => pg.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(pg => pg.ProductId);
        builder.HasIndex(pg => new { pg.ProductId, pg.SortOrder });
    }
}
