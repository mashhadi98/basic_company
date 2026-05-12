using Company.Application.Abstractions;
using Company.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Company.Infrastructure.Services;

/// <summary>
/// با تغییر SecurityStamp، اعتبار کوکی کاربر از بین می‌رود و باید مجدداً وارد شود تا Claimهای مجوز به‌روز شوند.
/// </summary>
public sealed class UserSecurityStampInvalidator : IUserSecurityStampInvalidator
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UserSecurityStampInvalidator(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InvalidateUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
        if (user is null)
        {
            return;
        }

        await _userManager.UpdateSecurityStampAsync(user).ConfigureAwait(false);
    }

    public async Task InvalidateUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        var role = await _roleManager.FindByNameAsync(roleName).ConfigureAwait(false);
        if (role is null)
        {
            return;
        }

        var users = await _userManager.GetUsersInRoleAsync(roleName).ConfigureAwait(false);
        foreach (var user in users)
        {
            await _userManager.UpdateSecurityStampAsync(user).ConfigureAwait(false);
        }
    }
}
