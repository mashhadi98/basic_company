using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.Persistence.Configurations;

public class ProductAttributeConfiguration : IEntityTypeConfiguration<ProductAttribute>
{
    public void Configure(EntityTypeBuilder<ProductAttribute> builder)
    {
        builder.ToTable("ProductAttributes");

        builder.HasKey(pa => pa.Id);

        // خیلی مهم برای Guid
        builder.Property(pa => pa.Id)
               .ValueGeneratedNever();           // EF خودش Id جدید تولید نکنه، ما خودمون Guid می‌دیم

        builder.Property(pa => pa.Key)
            .HasMaxLength(256)
            .IsRequired();

        builder.Property(pa => pa.Value)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(pa => pa.SortOrder)
            .HasDefaultValue(0);

        // Foreign Key
        builder.HasOne(pa => pa.Product)
            .WithMany(p => p.Attributes)
            .HasForeignKey(pa => pa.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(pa => pa.ProductId);
        builder.HasIndex(pa => new { pa.ProductId, pa.SortOrder });
    }
}
