using Microsoft.AspNetCore.Identity;

namespace Company.Infrastructure.Persistence.Identity;

/// <summary>
/// کاربر برنامه با فیلدهای اضافه‌شده نسبت به IdentityUser.
/// نام، نام خانوادگی، شماره تماس و ایمیل در Fluent API اجبار شده‌اند.
/// </summary>
public sealed class ApplicationUser : IdentityUser
{
    /// <summary>نام (اجباری).</summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>نام خانوادگی (اجباری).</summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>نام کامل محاسبه‌شده برای نمایش.</summary>
    public string FullName => $"{FirstName} {LastName}".Trim();
}
