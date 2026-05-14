using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.Persistence.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.CompanyName)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(c => c.CompanyImage)
            .HasMaxLength(512);

        builder.Property(c => c.SortOrder)
            .HasDefaultValue(0);

        builder.Property(c => c.IsPublished)
            .HasDefaultValue(false);

        builder.Property(c => c.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Property(c => c.CreatedBy)
            .HasMaxLength(256);

        builder.Property(c => c.UpdatedBy)
            .HasMaxLength(256);

        builder.HasIndex(c => c.SortOrder);
        builder.HasIndex(c => c.IsPublished);
    }
}
