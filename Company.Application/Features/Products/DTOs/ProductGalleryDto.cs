namespace Company.Application.Features.Products.DTOs;

/// <summary>
/// DTO برای گالری محصول
/// </summary>
public class ProductGalleryDto
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
}

/// <summary>
/// DTO برای ایجاد یا ویرایش گالری محصول
/// </summary>
public class CreateOrUpdateProductGalleryDto
{
    public string ImageUrl { get; set; } = string.Empty;
    public string? AltText { get; set; }
    public int SortOrder { get; set; }
    public bool IsPrimary { get; set; }
}
