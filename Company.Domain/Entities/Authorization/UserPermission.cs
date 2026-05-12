namespace Company.Domain.Entities.Authorization;

/// <summary>
/// تخصیص مستقیم مجوز به کاربر (علاوه بر مجوزهای نقش‌ها).
/// </summary>
public sealed class UserPermission
{
    public string UserId { get; set; } = string.Empty;
    public int PermissionId { get; set; }

    public Permission Permission { get; set; } = null!;
}
