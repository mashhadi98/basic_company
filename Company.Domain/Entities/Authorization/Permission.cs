namespace Company.Domain.Entities.Authorization;

/// <summary>
/// یک مجوز داینامیک در سیستم (مثلاً Product.Create). مقدار Name معمولاً به صورت Namespace.Action است.
/// </summary>
public sealed class Permission
{
    public int Id { get; set; }

    /// <summary>نام یکتا برای استفاده در کد و Claim (مثلاً Product.Create).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>عنوان نمایشی برای UI.</summary>
    public string DisplayName { get; set; } = string.Empty;

    public string? Description { get; set; }

    /// <summary>گروه‌بندی در پنل مدیریت (مثلاً Product، Users).</summary>
    public string Group { get; set; } = string.Empty;

    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
