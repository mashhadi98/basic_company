using Company.Application.Abstractions;
using Company.Application.Authorization;
using Company.Domain.Entities.Authorization;
using Company.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Services;

/// <summary>
/// عملیات CRUD منطقی روی مجوزها و نقش‌ها؛ پس از هر تغییر، نشست کاربران تحت تأثیر ابطال می‌شود.
/// </summary>
public sealed class PermissionManagementService : IPermissionManagementService
{
    private readonly AppDbContext _dbContext;
    private readonly IUserSecurityStampInvalidator _stampInvalidator;

    public PermissionManagementService(AppDbContext dbContext, IUserSecurityStampInvalidator stampInvalidator)
    {
        _dbContext = dbContext;
        _stampInvalidator = stampInvalidator;
    }

    public async Task<IReadOnlyList<PermissionDto>> GetAllPermissionsAsync(CancellationToken cancellationToken = default)
    {
        var list = await _dbContext.Permissions.AsNoTracking()
            .OrderBy(p => p.Group)
            .ThenBy(p => p.Name)
            .Select(p => new PermissionDto(p.Id, p.Name, p.DisplayName, p.Description, p.Group))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return list;
    }

    public async Task AssignPermissionToRoleAsync(string roleName, string permissionName, CancellationToken cancellationToken = default)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Role '{roleName}' not found.");

        var permission = await _dbContext.Permissions.FirstOrDefaultAsync(p => p.Name == permissionName, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Permission '{permissionName}' not found.");

        var exists = await _dbContext.RolePermissions.AnyAsync(
            rp => rp.RoleId == role.Id && rp.PermissionId == permission.Id,
            cancellationToken).ConfigureAwait(false);

        if (!exists)
        {
            _dbContext.RolePermissions.Add(new RolePermission
            {
                RoleId = role.Id,
                PermissionId = permission.Id,
            });
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await _stampInvalidator.InvalidateUsersInRoleAsync(roleName, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task RemovePermissionFromRoleAsync(string roleName, string permissionName, CancellationToken cancellationToken = default)
    {
        var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == roleName, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Role '{roleName}' not found.");

        var permission = await _dbContext.Permissions.FirstOrDefaultAsync(p => p.Name == permissionName, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Permission '{permissionName}' not found.");

        var link = await _dbContext.RolePermissions.FirstOrDefaultAsync(
            rp => rp.RoleId == role.Id && rp.PermissionId == permission.Id,
            cancellationToken).ConfigureAwait(false);

        if (link is not null)
        {
            _dbContext.RolePermissions.Remove(link);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await _stampInvalidator.InvalidateUsersInRoleAsync(roleName, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task AssignPermissionToUserAsync(string userId, string permissionName, CancellationToken cancellationToken = default)
    {
        var permission = await _dbContext.Permissions.FirstOrDefaultAsync(p => p.Name == permissionName, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Permission '{permissionName}' not found.");

        var exists = await _dbContext.UserPermissions.AnyAsync(
            up => up.UserId == userId && up.PermissionId == permission.Id,
            cancellationToken).ConfigureAwait(false);

        if (!exists)
        {
            _dbContext.UserPermissions.Add(new UserPermission
            {
                UserId = userId,
                PermissionId = permission.Id,
            });
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await _stampInvalidator.InvalidateUserAsync(userId, cancellationToken).ConfigureAwait(false);
        }
    }

    public async Task RemovePermissionFromUserAsync(string userId, string permissionName, CancellationToken cancellationToken = default)
    {
        var permission = await _dbContext.Permissions.FirstOrDefaultAsync(p => p.Name == permissionName, cancellationToken).ConfigureAwait(false)
            ?? throw new InvalidOperationException($"Permission '{permissionName}' not found.");

        var link = await _dbContext.UserPermissions.FirstOrDefaultAsync(
            up => up.UserId == userId && up.PermissionId == permission.Id,
            cancellationToken).ConfigureAwait(false);

        if (link is not null)
        {
            _dbContext.UserPermissions.Remove(link);
            await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            await _stampInvalidator.InvalidateUserAsync(userId, cancellationToken).ConfigureAwait(false);
        }
    }
}
