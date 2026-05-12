namespace Company.Application.Abstractions;

/// <summary>
/// پس از تغییر مجوزها، با به‌روزرسانی SecurityStamp کاربر، اعتبار کوکی منقضی می‌شود و کاربر باید دوباره وارد شود (Claims تازه می‌گیرد).
/// </summary>
public interface IUserSecurityStampInvalidator
{
    Task InvalidateUserAsync(string userId, CancellationToken cancellationToken = default);

    Task InvalidateUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default);
}
