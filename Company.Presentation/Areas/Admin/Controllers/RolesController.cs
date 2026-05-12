using Company.Domain.Entities.Authorization;
using Company.Infrastructure.Persistence;
using Company.Infrastructure.Persistence.Identity;
using Company.Presentation.Areas.Admin.Models.Roles;
using Company.Presentation.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[HasPermission("Roles.View")]
public sealed class RolesController : Controller
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly AppDbContext _dbContext;

    public RolesController(RoleManager<ApplicationRole> roleManager, AppDbContext dbContext)
    {
        _roleManager = roleManager;
        _dbContext = dbContext;
    }

    public IActionResult Index()
    {
        var roles = _roleManager.Roles
            .OrderBy(r => r.Name)
            .AsEnumerable()
            .Select(RoleListItemViewModel.FromRole)
            .ToList();

        return View(roles);
    }

    [HasPermission("Roles.Manage")]
    [HttpGet]
    public IActionResult Create() => View("Edit", new RoleCreateEditViewModel());

    [HasPermission("Roles.Manage")]
    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var role = await _roleManager.FindByIdAsync(id).ConfigureAwait(false);
        if (role is null)
        {
            return NotFound();
        }

        return View(new RoleCreateEditViewModel
        {
            Id = role.Id,
            Name = role.Name ?? string.Empty,
            Description = role.Description,
        });
    }

    [HasPermission("Roles.Manage")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(RoleCreateEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (string.IsNullOrEmpty(model.Id))
        {
            var role = new ApplicationRole { Name = model.Name, Description = model.Description };
            var create = await _roleManager.CreateAsync(role).ConfigureAwait(false);
            if (!create.Succeeded)
            {
                foreach (var error in create.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
        else
        {
            var role = await _roleManager.FindByIdAsync(model.Id).ConfigureAwait(false);
            if (role is null)
            {
                return NotFound();
            }

            role.Name = model.Name;
            role.Description = model.Description;

            var update = await _roleManager.UpdateAsync(role).ConfigureAwait(false);
            if (!update.Succeeded)
            {
                foreach (var error in update.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }

    [HasPermission("Roles.Manage")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(string id)
    {
        var role = await _roleManager.FindByIdAsync(id).ConfigureAwait(false);
        if (role is null)
        {
            return NotFound();
        }

        var result = await _roleManager.DeleteAsync(role).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return RedirectToAction(nameof(Index));
    }

    [HasPermission("Roles.Manage")]
    [HttpGet]
    public async Task<IActionResult> Permissions(string id)
    {
        var role = await _roleManager.FindByIdAsync(id).ConfigureAwait(false);
        if (role is null)
        {
            return NotFound();
        }

        var allPermissions = await _dbContext.Permissions.AsNoTracking()
            .OrderBy(p => p.Group)
            .ThenBy(p => p.Name)
            .Select(p => new RolePermissionsViewModel.PermissionItem(p.Id, p.Name, p.DisplayName, p.Group))
            .ToListAsync()
            .ConfigureAwait(false);

        var assigned = await _dbContext.RolePermissions.AsNoTracking()
            .Where(rp => rp.RoleId == role.Id)
            .Select(rp => rp.Permission.Name)
            .ToListAsync()
            .ConfigureAwait(false);

        return View(new RolePermissionsViewModel
        {
            RoleId = role.Id,
            RoleName = role.Name ?? string.Empty,
            AllPermissions = allPermissions,
            AssignedPermissionNames = new HashSet<string>(assigned, StringComparer.OrdinalIgnoreCase),
        });
    }

    [HasPermission("Roles.Manage")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Permissions(string roleId, string[] permissionNames)
    {
        var role = await _roleManager.FindByIdAsync(roleId).ConfigureAwait(false);
        if (role is null)
        {
            return NotFound();
        }

        var desired = new HashSet<string>(permissionNames ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);

        var permissions = await _dbContext.Permissions.AsNoTracking()
            .Select(p => new { p.Id, p.Name })
            .ToListAsync()
            .ConfigureAwait(false);

        var current = await _dbContext.RolePermissions
            .Where(rp => rp.RoleId == role.Id)
            .Select(rp => new { rp.PermissionId, Name = rp.Permission.Name })
            .ToListAsync()
            .ConfigureAwait(false);

        var currentNames = new HashSet<string>(current.Select(x => x.Name), StringComparer.OrdinalIgnoreCase);
        var toAddNames = desired.Except(currentNames, StringComparer.OrdinalIgnoreCase).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var toRemoveNames = currentNames.Except(desired, StringComparer.OrdinalIgnoreCase).ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (toAddNames.Count > 0)
        {
            foreach (var p in permissions.Where(p => toAddNames.Contains(p.Name)))
            {
                _dbContext.RolePermissions.Add(new RolePermission { RoleId = role.Id, PermissionId = p.Id });
            }
        }

        if (toRemoveNames.Count > 0)
        {
            var remove = current.Where(x => toRemoveNames.Contains(x.Name)).ToList();
            foreach (var item in remove)
            {
                _dbContext.RolePermissions.Remove(new RolePermission { RoleId = role.Id, PermissionId = item.PermissionId });
            }
        }

        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        return RedirectToAction(nameof(Permissions), new { id = role.Id });
    }
}

