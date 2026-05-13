using Company.Domain.Common;

namespace Company.Domain.Entities;

/// <summary>
/// دسته‌بندی محصول - پشتیبانی از دسته‌بندی‌های سلسله‌مراتبی
/// </summary>
public class ProductCategory : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string? Image { get; set; }
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; }

    // SEO Fields
    public string? SeoMetaTitle { get; set; }
    public string? SeoMetaDescription { get; set; }

    // Audit Information
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation Properties
    public ProductCategory? ParentCategory { get; set; }
    public ICollection<ProductCategory> ChildCategories { get; set; } = new List<ProductCategory>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
