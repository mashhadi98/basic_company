using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.Persistence.Configurations;

public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
{
    public void Configure(EntityTypeBuilder<ProductTag> builder)
    {
        builder.ToTable("ProductTags");

        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(pt => pt.Slug)
            .HasMaxLength(256)
            .IsRequired();

        // Foreign Key
        builder.HasOne(pt => pt.Product)
            .WithMany(p => p.Tags)
            .HasForeignKey(pt => pt.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(pt => pt.ProductId);
        builder.HasIndex(pt => pt.Slug);
    }
}
