using Company.Domain.Entities;

namespace Company.Application.Features.Products.DTOs;

/// <summary>
/// DTO برای دسته‌بندی محصول
/// </summary>
public class ProductCategoryDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string? Image { get; set; }
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; }
    public string? SeoMetaTitle { get; set; }
    public string? SeoMetaDescription { get; set; }
}

/// <summary>
/// DTO برای ایجاد یا ویرایش دسته‌بندی محصول
/// </summary>
public class CreateOrUpdateProductCategoryDto
{
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string? Image { get; set; }
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; }
    public string? SeoMetaTitle { get; set; }
    public string? SeoMetaDescription { get; set; }
}
