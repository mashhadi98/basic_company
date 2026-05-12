using Company.Presentation.Areas.Admin.Models.Users;
using Company.Presentation.Authorization;
using Company.Infrastructure.Persistence.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[HasPermission("Users.View")]
public sealed class UsersController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public UsersController(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        var users = _userManager.Users
            .OrderBy(u => u.UserName)
            .AsEnumerable()
            .Select(UserListItemViewModel.FromUser)
            .ToList();

        return View(users);
    }

    [HasPermission("Users.Manage")]
    [HttpGet]
    public IActionResult Create() => View(new UserCreateViewModel());

    [HasPermission("Users.Manage")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(UserCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        if (await _userManager.FindByNameAsync(model.Username).ConfigureAwait(false) is not null)
        {
            ModelState.AddModelError(nameof(UserCreateViewModel.Username), "این نام کاربری قبلاً استفاده شده است.");
            return View(model);
        }

        var user = new ApplicationUser
        {
            UserName = model.Username,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
        };

        var result = await _userManager.CreateAsync(user, model.Password).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HasPermission("Users.Manage")]
    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
        if (user is null)
        {
            return NotFound();
        }

        return View(new UserEditViewModel
        {
            Id = user.Id,
            Username = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber ?? string.Empty,
            EmailConfirmed = user.EmailConfirmed,
            PhoneNumberConfirmed = user.PhoneNumberConfirmed,
        });
    }

    [HasPermission("Users.Manage")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UserEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByIdAsync(model.Id).ConfigureAwait(false);
        if (user is null)
        {
            return NotFound();
        }

        if (!string.Equals(user.UserName, model.Username, StringComparison.OrdinalIgnoreCase))
        {
            if (await _userManager.FindByNameAsync(model.Username).ConfigureAwait(false) is not null)
            {
                ModelState.AddModelError(nameof(UserEditViewModel.Username), "این نام کاربری قبلاً استفاده شده است.");
                return View(model);
            }
        }

        user.UserName = model.Username;
        user.Email = model.Email;
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;
        user.EmailConfirmed = model.EmailConfirmed;
        user.PhoneNumberConfirmed = model.PhoneNumberConfirmed;

        var result = await _userManager.UpdateAsync(user).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HasPermission("Users.Manage")]
    [HttpGet]
    public async Task<IActionResult> Roles(string id)
    {
        var user = await _userManager.FindByIdAsync(id).ConfigureAwait(false);
        if (user is null)
        {
            return NotFound();
        }

        var assigned = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        var allRoles = _roleManager.Roles
            .OrderBy(r => r.Name)
            .AsEnumerable()
            .Select(r => new UserRolesViewModel.RoleItem(r.Name ?? string.Empty, r.Description))
            .Where(r => !string.IsNullOrWhiteSpace(r.Name))
            .ToList();

        return View(new UserRolesViewModel
        {
            UserId = user.Id,
            UserName = user.UserName ?? string.Empty,
            AllRoles = allRoles,
            AssignedRoles = new HashSet<string>(assigned, StringComparer.OrdinalIgnoreCase),
        });
    }

    [HasPermission("Users.Manage")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Roles(string userId, string[] roles)
    {
        var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
        if (user is null)
        {
            return NotFound();
        }

        var current = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
        var desired = new HashSet<string>(roles ?? Array.Empty<string>(), StringComparer.OrdinalIgnoreCase);

        var toAdd = desired.Except(current, StringComparer.OrdinalIgnoreCase).ToArray();
        var toRemove = current.Except(desired, StringComparer.OrdinalIgnoreCase).ToArray();

        if (toAdd.Length > 0)
        {
            var addResult = await _userManager.AddToRolesAsync(user, toAdd).ConfigureAwait(false);
            if (!addResult.Succeeded)
            {
                foreach (var error in addResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }

        if (toRemove.Length > 0)
        {
            var removeResult = await _userManager.RemoveFromRolesAsync(user, toRemove).ConfigureAwait(false);
            if (!removeResult.Succeeded)
            {
                foreach (var error in removeResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }

        if (!ModelState.IsValid)
        {
            return await Roles(userId).ConfigureAwait(false);
        }

        return RedirectToAction(nameof(Index));
    }
}

