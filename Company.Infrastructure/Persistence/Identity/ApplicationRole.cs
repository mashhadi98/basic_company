using Microsoft.AspNetCore.Identity;

namespace Company.Infrastructure.Persistence.Identity;

/// <summary>
/// نقش برنامه؛ می‌توانید در آینده توضیح یا متادیتا اضافه کنید.
/// </summary>
public sealed class ApplicationRole : IdentityRole
{
    /// <summary>توضیح اختیاری نقش برای UI.</summary>
    public string? Description { get; set; }
}
