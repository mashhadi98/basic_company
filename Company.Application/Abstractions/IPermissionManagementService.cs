using Company.Application.Authorization;

namespace Company.Application.Abstractions;

/// <summary>
/// مدیریت کاتالوگ مجوزها و تخصیص به نقش/کاربر + ابطال نشست پس از تغییر.
/// </summary>
public interface IPermissionManagementService
{
    Task<IReadOnlyList<PermissionDto>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);

    Task AssignPermissionToRoleAsync(string roleName, string permissionName, CancellationToken cancellationToken = default);

    Task RemovePermissionFromRoleAsync(string roleName, string permissionName, CancellationToken cancellationToken = default);

    Task AssignPermissionToUserAsync(string userId, string permissionName, CancellationToken cancellationToken = default);

    Task RemovePermissionFromUserAsync(string userId, string permissionName, CancellationToken cancellationToken = default);
}
