namespace Company.Presentation.Areas.Admin.Models;

public class BlogCategoryViewModel
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
