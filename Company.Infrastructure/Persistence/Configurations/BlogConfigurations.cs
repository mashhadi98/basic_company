using Company.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Company.Infrastructure.Persistence.Configurations;

public class BlogCategoryConfiguration : IEntityTypeConfiguration<BlogCategory>
{
    public void Configure(EntityTypeBuilder<BlogCategory> builder)
    {
        builder.ToTable("BlogCategories");
        builder.HasKey(bc => bc.Id);

        builder.Property(bc => bc.Title)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(bc => bc.Slug)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(bc => bc.Description)
            .HasMaxLength(1000)
            .IsRequired();

        builder.Property(bc => bc.SortOrder)
            .HasDefaultValue(0);

        builder.Property(bc => bc.IsPublished)
            .HasDefaultValue(false);

        builder.Property(bc => bc.SeoMetaTitle)
            .HasMaxLength(60);

        builder.Property(bc => bc.SeoMetaDescription)
            .HasMaxLength(160);

        // Indexes
        builder.HasIndex(bc => bc.Slug).IsUnique();
        builder.HasIndex(bc => bc.IsPublished);
        builder.HasIndex(bc => bc.SortOrder);
    }
}

public class BlogPostConfiguration : IEntityTypeConfiguration<BlogPost>
{
    public void Configure(EntityTypeBuilder<BlogPost> builder)
    {
        builder.ToTable("BlogPosts");
        builder.HasKey(bp => bp.Id);

        builder.Property(bp => bp.Title)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(bp => bp.Slug)
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(bp => bp.Content)
            .IsRequired();

        builder.Property(bp => bp.Summary)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(bp => bp.Author)
            .HasMaxLength(200);

        builder.Property(bp => bp.ViewCount)
            .HasDefaultValue(0);

        builder.Property(bp => bp.SortOrder)
            .HasDefaultValue(0);

        builder.Property(bp => bp.IsPublished)
            .HasDefaultValue(false);

        builder.Property(bp => bp.AllowComments)
            .HasDefaultValue(true);

        builder.Property(bp => bp.SeoMetaTitle)
            .HasMaxLength(60);

        builder.Property(bp => bp.SeoMetaDescription)
            .HasMaxLength(160);

        builder.HasOne(bp => bp.Category)
            .WithMany(bc => bc.Posts)
            .HasForeignKey(bp => bp.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(bp => bp.Slug).IsUnique();
        builder.HasIndex(bp => bp.CategoryId);
        builder.HasIndex(bp => bp.IsPublished);
        builder.HasIndex(bp => bp.PublishedDate).IsDescending();
    }
}

public class BlogCommentConfiguration : IEntityTypeConfiguration<BlogComment>
{
    public void Configure(EntityTypeBuilder<BlogComment> builder)
    {
        builder.ToTable("BlogComments");
        builder.HasKey(bc => bc.Id);

        builder.Property(bc => bc.AuthorName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(bc => bc.AuthorEmail)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(bc => bc.Content)
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(bc => bc.IsApproved)
            .HasDefaultValue(false);

        builder.Property(bc => bc.SortOrder)
            .HasDefaultValue(0);

        builder.HasOne(bc => bc.BlogPost)
            .WithMany(bp => bp.Comments)
            .HasForeignKey(bc => bc.BlogPostId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(bc => bc.BlogPostId);
        builder.HasIndex(bc => bc.IsApproved);
        builder.HasIndex(bc => bc.CreatedAt).IsDescending();
    }
}
