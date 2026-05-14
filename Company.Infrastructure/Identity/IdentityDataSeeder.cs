using Company.Domain.Entities.Authorization;
using Company.Infrastructure.Persistence;
using Company.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Company.Infrastructure.Identity;

/// <summary>
/// ایجاد مجوزهای پایه، نقش Admin و کاربر ادمین (در صورت نبود).
/// </summary>
public static class IdentityDataSeeder
{
    private static readonly PermissionDefinition[] PermissionDefinitions =
    [
        new("Users.View", "مشاهده کاربران", "لیست و جزئیات کاربران", "Users"),
        new("Users.Manage", "مدیریت کاربران", "ایجاد/ویرایش/حذف کاربران", "Users"),
        new("Roles.View", "مشاهده نقش‌ها", "لیست نقش‌ها", "Roles"),
        new("Roles.Manage", "مدیریت نقش‌ها", "تخصیص مجوز به نقش‌ها", "Roles"),
        new("Permissions.View", "مشاهده مجوزها", "لیست مجوزها", "Permissions"),
        new("Product.View", "مشاهده محصولات", "لیست محصولات", "Product"),
        new("Product.Create", "ایجاد محصول", "افزودن محصول جدید", "Product"),
        new("Product.Edit", "ویرایش محصول", "ویرایش محصول موجود", "Product"),
        new("Product.Delete", "حذف محصول", "حذف محصول", "Product"),
        new("Customer.View", "مشاهده مشتریان", "لیست مشتریان", "Customer"),
        new("Customer.Create", "ایجاد مشتری", "افزودن مشتری جدید", "Customer"),
        new("Customer.Edit", "ویرایش مشتری", "ویرایش مشتری موجود", "Customer"),
        new("Customer.Delete", "حذف مشتری", "حذف مشتری", "Customer"),
    ];

    public static async Task SeedAsync(IServiceProvider services, CancellationToken cancellationToken = default)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;

        var db = provider.GetRequiredService<AppDbContext>();
        var roleManager = provider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
        var seedOptions = provider.GetRequiredService<IOptions<IdentitySeedOptions>>().Value;

        await SeedPermissionsAsync(db, cancellationToken).ConfigureAwait(false);
        await EnsureAdminRoleAsync(roleManager, seedOptions, cancellationToken).ConfigureAwait(false);
        await AssignAllPermissionsToAdminRoleAsync(db, roleManager, seedOptions, cancellationToken).ConfigureAwait(false);
        await EnsureAdminUserAsync(userManager, seedOptions, cancellationToken).ConfigureAwait(false);
    }

    private static async Task SeedPermissionsAsync(AppDbContext db, CancellationToken cancellationToken)
    {
        var existing = await db.Permissions.Select(p => p.Name).ToListAsync(cancellationToken).ConfigureAwait(false);
        foreach (var def in PermissionDefinitions)
        {
            if (existing.Contains(def.Name))
            {
                continue;
            }

            db.Permissions.Add(new Permission
            {
                Name = def.Name,
                DisplayName = def.DisplayName,
                Description = def.Description,
                Group = def.Group,
            });
        }

        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private static async Task EnsureAdminRoleAsync(
        RoleManager<ApplicationRole> roleManager,
        IdentitySeedOptions seedOptions,
        CancellationToken cancellationToken)
    {
        var roleName = seedOptions.AdminRoleName;
        var role = await roleManager.FindByNameAsync(roleName).ConfigureAwait(false);
        if (role is not null)
        {
            return;
        }

        var result = await roleManager.CreateAsync(new ApplicationRole
        {
            Name = roleName,
            NormalizedName = roleName.ToUpperInvariant(),
            Description = "دسترسی کامل مدیریتی",
        }).ConfigureAwait(false);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException($"Cannot create admin role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }

    private static async Task AssignAllPermissionsToAdminRoleAsync(
        AppDbContext db,
        RoleManager<ApplicationRole> roleManager,
        IdentitySeedOptions seedOptions,
        CancellationToken cancellationToken)
    {
        var role = await roleManager.FindByNameAsync(seedOptions.AdminRoleName).ConfigureAwait(false);
        if (role is null)
        {
            return;
        }

        var permissionIds = await db.Permissions.Select(p => p.Id).ToListAsync(cancellationToken).ConfigureAwait(false);
        var existingPairs = await db.RolePermissions
            .Where(rp => rp.RoleId == role.Id)
            .Select(rp => rp.PermissionId)
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        foreach (var pid in permissionIds.Where(pid => !existingPairs.Contains(pid)))
        {
            db.RolePermissions.Add(new RolePermission
            {
                RoleId = role.Id,
                PermissionId = pid,
            });
        }

        await db.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
    }

    private static async Task EnsureAdminUserAsync(
        UserManager<ApplicationUser> userManager,
        IdentitySeedOptions seedOptions,
        CancellationToken cancellationToken)
    {
        var userName = seedOptions.AdminUserName;
        var email = seedOptions.AdminEmail;

        var existing = await userManager.FindByNameAsync(userName).ConfigureAwait(false)
            ?? await userManager.FindByEmailAsync(email).ConfigureAwait(false);
        if (existing is not null)
        {
            await EnsureAdminRoleAssignedAsync(userManager, existing, seedOptions.AdminRoleName).ConfigureAwait(false);
            return;
        }

        var user = new ApplicationUser
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            PhoneNumber = seedOptions.AdminPhoneNumber,
            PhoneNumberConfirmed = true,
            FirstName = seedOptions.AdminFirstName,
            LastName = seedOptions.AdminLastName,
        };

        var create = await userManager.CreateAsync(user, seedOptions.AdminPassword).ConfigureAwait(false);
        if (!create.Succeeded)
        {
            throw new InvalidOperationException($"Cannot create admin user: {string.Join(", ", create.Errors.Select(e => e.Description))}");
        }

        await EnsureAdminRoleAssignedAsync(userManager, user, seedOptions.AdminRoleName).ConfigureAwait(false);
    }

    private static async Task EnsureAdminRoleAssignedAsync(
        UserManager<ApplicationUser> userManager,
        ApplicationUser user,
        string adminRoleName)
    {
        if (!await userManager.IsInRoleAsync(user, adminRoleName).ConfigureAwait(false))
        {
            var addRole = await userManager.AddToRoleAsync(user, adminRoleName).ConfigureAwait(false);
            if (!addRole.Succeeded)
            {
                throw new InvalidOperationException($"Cannot assign admin role: {string.Join(", ", addRole.Errors.Select(e => e.Description))}");
            }
        }
    }

    private sealed record PermissionDefinition(string Name, string DisplayName, string Description, string Group);
}
