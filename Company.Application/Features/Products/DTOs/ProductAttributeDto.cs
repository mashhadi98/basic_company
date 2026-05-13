namespace Company.Application.Features.Products.DTOs;

/// <summary>
/// DTO برای ویژگی محصول
/// </summary>
public class ProductAttributeDto
{
    public Guid? Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}

/// <summary>
/// DTO برای ایجاد یا ویرایش ویژگی محصول
/// </summary>
public class CreateOrUpdateProductAttributeDto
{
    public Guid? Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public int SortOrder { get; set; }
}
