namespace Company.Presentation.Areas.Admin.Models;

/// <summary>
/// ViewModel برای مدیریت صفحات ثابت در پنل ادمین
/// </summary>
public class StaticPageViewModel
{
    /// <summary>
    /// شناسهٔ منحصر به فرد صفحه
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// کلید منحصر به فرد برای شناسایی صفحه
    /// </summary>
    public string Key { get; set; } = string.Empty;

    /// <summary>
    /// عنوان صفحه
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// خلاصهٔ صفحه برای نمایش در لیست‌ها
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// توضیحات کامل صفحه
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// مسیر یا URL تصویر صفحه
    /// </summary>
    public string? Image { get; set; }

    /// <summary>
    /// فایل تصویر برای آپلود
    /// </summary>
    public IFormFile? ImageFile { get; set; }

    /// <summary>
    /// وضعیت انتشار صفحه
    /// </summary>
    public bool IsPublished { get; set; }
}