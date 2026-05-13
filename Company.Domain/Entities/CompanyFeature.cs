using Company.Domain.Common;

namespace Company.Domain.Entities;

/// <summary>
/// ویژگی‌های شرکت - مزایا، چرا ما را انتخاب کنید، نکات برجسته خدمات
/// </summary>
public class CompanyFeature : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty; // کلاس آیکون مثل fa-solid fa-star
    public int SortOrder { get; set; }
    public bool IsPublished { get; set; }
}