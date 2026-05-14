using Company.Domain.Common;

namespace Company.Domain.Entities;

public class BlogCategory : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; }

    // SEO
    public string? SeoMetaTitle { get; set; }
    public string? SeoMetaDescription { get; set; }
    public string? SeoKeywords { get; set; }
    public string? CanonicalUrl { get; set; }

    // Audit Information
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public ICollection<BlogPost> Posts { get; set; } = new List<BlogPost>();
}

public class BlogPost : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string? FeaturedImage { get; set; }
    public Guid CategoryId { get; set; }
    public string? Author { get; set; }
    public DateTime PublishedDate { get; set; }
    public bool IsPublished { get; set; }
    public bool AllowComments { get; set; } = true;
    public int ViewCount { get; set; }
    public int SortOrder { get; set; }

    // SEO
    public string? SeoMetaTitle { get; set; }
    public string? SeoMetaDescription { get; set; }
    public string? SeoKeywords { get; set; }
    public string? CanonicalUrl { get; set; }

    // Audit Information
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public BlogCategory? Category { get; set; }
    public ICollection<BlogComment> Comments { get; set; } = new List<BlogComment>();
}

public class BlogComment : BaseEntity
{
    public string AuthorName { get; set; } = string.Empty;
    public string AuthorEmail { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid BlogPostId { get; set; }
    public bool IsApproved { get; set; }
    public int SortOrder { get; set; }

    // Audit Information
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public BlogPost? BlogPost { get; set; }
}
