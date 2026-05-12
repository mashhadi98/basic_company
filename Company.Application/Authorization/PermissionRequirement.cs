using Microsoft.AspNetCore.Authorization;

namespace Company.Application.Authorization;

/// <summary>
/// الزام مجوز بر اساس نام مجوز (با پیشوند Policy برای یکتا بودن نام سیاست‌ها).
/// </summary>
public sealed class PermissionRequirement : IAuthorizationRequirement
{
    /// <summary>پیشوند یکسان با PermissionPolicyProvider.</summary>
    public const string PolicyPrefix = "Permission:";

    public PermissionRequirement(string permissionName)
    {
        PermissionName = permissionName ?? throw new ArgumentNullException(nameof(permissionName));
    }

    /// <summary>نام مجوز بدون پیشوند (مثلاً Product.Create).</summary>
    public string PermissionName { get; }
}
