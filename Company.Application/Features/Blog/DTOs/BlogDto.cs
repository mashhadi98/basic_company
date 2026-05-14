using Company.Domain.Entities;

namespace Company.Application.Features.Blog.DTOs;

public class BlogCategoryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; }
    public string? SeoMetaTitle { get; set; }
    public string? SeoMetaDescription { get; set; }
    public string? SeoKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateOrUpdateBlogCategoryDto
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; }
    public string? SeoMetaTitle { get; set; }
    public string? SeoMetaDescription { get; set; }
    public string? SeoKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
}

public class BlogPostDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string? FeaturedImage { get; set; }
    public Guid CategoryId { get; set; }
    public string? Author { get; set; }
    public DateTime PublishedDate { get; set; }
    public bool IsPublished { get; set; }
    public bool AllowComments { get; set; }
    public int ViewCount { get; set; }
    public int SortOrder { get; set; }
    public string? SeoMetaTitle { get; set; }
    public string? SeoMetaDescription { get; set; }
    public string? SeoKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<BlogCommentDto> Comments { get; set; } = new();
}

public class CreateOrUpdateBlogPostDto
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
    public int SortOrder { get; set; }
    public string? SeoMetaTitle { get; set; }
    public string? SeoMetaDescription { get; set; }
    public string? SeoKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
}

public class BlogCommentDto
{
    public Guid Id { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string AuthorEmail { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid BlogPostId { get; set; }
    public bool IsApproved { get; set; }
    public int SortOrder { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateBlogCommentDto
{
    public string AuthorName { get; set; } = string.Empty;
    public string AuthorEmail { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid BlogPostId { get; set; }
}
