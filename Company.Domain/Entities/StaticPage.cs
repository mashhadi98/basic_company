using Company.Domain.Common;

namespace Company.Domain.Entities;

/// <summary>
/// صفحات ثابت - درباره ما، قوانین مقررات، تماس با ما و غیره
/// </summary>
public class StaticPage : BaseEntity
{
    public string Key { get; set; } = string.Empty; // کلید منحصر به فرد مثل "aboutus"
    public string Title { get; set; } = string.Empty; // عنوان صفحه
    public string Summary { get; set; } = string.Empty; // خلاصه کوتاه
    public string Description { get; set; } = string.Empty; // توضیحات کامل
    public string? Image { get; set; } // مسیر تصویر
    public bool IsPublished { get; set; }

    // Audit Information
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}