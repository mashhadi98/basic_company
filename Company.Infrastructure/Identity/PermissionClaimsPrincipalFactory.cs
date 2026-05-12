using System.Security.Claims;
using Company.Application.Authorization;
using Company.Domain.Entities.Authorization;
using Company.Infrastructure.Persistence;
using Company.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Company.Infrastructure.Identity;

/// <summary>
/// هنگام صدور Principal، تمام مجوزهای نقش‌ها و تخصیص مستقیم کاربر را به صورت Claim اضافه می‌کند.
/// </summary>
public sealed class PermissionClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, ApplicationRole>
{
    private readonly AppDbContext _dbContext;

    public PermissionClaimsPrincipalFactory(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IOptions<IdentityOptions> optionsAccessor,
        AppDbContext dbContext)
        : base(userManager, roleManager, optionsAccessor)
    {
        _dbContext = dbContext;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        var roleNames = await UserManager.GetRolesAsync(user);
        var roleIds = await _dbContext.Roles.AsNoTracking()
            .Where(r => r.Name != null && roleNames.Contains(r.Name))
            .Select(r => r.Id)
            .ToListAsync();

        var fromRoles = await _dbContext.RolePermissions.AsNoTracking()
            .Where(rp => roleIds.Contains(rp.RoleId))
            .Select(rp => rp.Permission.Name)
            .ToListAsync();

        var direct = await _dbContext.UserPermissions.AsNoTracking()
            .Where(up => up.UserId == user.Id)
            .Select(up => up.Permission.Name)
            .ToListAsync();

        foreach (var name in fromRoles.Concat(direct).Distinct(StringComparer.Ordinal))
        {
            identity.AddClaim(new Claim(AuthClaimTypes.Permission, name));
        }

        return identity;
    }
}
