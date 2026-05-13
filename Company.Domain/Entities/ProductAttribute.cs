using Company.Domain.Common;

namespace Company.Domain.Entities;

/// <summary>
/// ویژگی‌های پویای محصول - سیستم کلید-مقدار برای مشخصات محصول
/// </summary>
public class ProductAttribute : BaseEntity
{
    public Guid ProductId { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int SortOrder { get; set; }

    // Navigation Properties
    public Product? Product { get; set; }
}
