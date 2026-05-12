using Company.Infrastructure.Persistence;
using Company.Presentation.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[HasPermission("Permissions.View")]
public sealed class PermissionsController : Controller
{
    private readonly AppDbContext _dbContext;

    public PermissionsController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _dbContext.Permissions.AsNoTracking()
            .OrderBy(p => p.Group)
            .ThenBy(p => p.Name)
            .ToListAsync()
            .ConfigureAwait(false);

        return View(list);
    }
}

