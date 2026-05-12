namespace Company.Domain.Entities.Authorization;

/// <summary>
/// رابطه چند‌به‌چند بین نقش (AspNetRoles) و مجوز.
/// </summary>
public sealed class RolePermission
{
    public string RoleId { get; set; } = string.Empty;
    public int PermissionId { get; set; }

    public Permission Permission { get; set; } = null!;
}
