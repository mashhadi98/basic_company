using Microsoft.AspNetCore.Http;

namespace Company.Presentation.Areas.Admin.Models;

public class BlogPostViewModel
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string? Author { get; set; }
    public DateTime PublishedDate { get; set; } = DateTime.UtcNow;
    public bool IsPublished { get; set; }
    public bool AllowComments { get; set; } = true;
    public int SortOrder { get; set; }
    public string? FeaturedImage { get; set; }
    public IFormFile? FeaturedImageFile { get; set; }
    public string? SeoMetaTitle { get; set; }
    public string? SeoMetaDescription { get; set; }
    public string? SeoKeywords { get; set; }
    public string? CanonicalUrl { get; set; }
}
