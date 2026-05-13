using Company.Application.Features.SiteSettings.Commands;
using Company.Application.Features.SiteSettings.DTOs;
using Company.Application.Features.SiteSettings.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class SiteSettingsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public SiteSettingsController(IMediator mediator, IWebHostEnvironment webHostEnvironment)
    {
        _mediator = mediator;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int skip = 0, int take = 10)
    {
        var query = new GetSiteSettingsQuery { Skip = skip, Take = take };
        var settings = await _mediator.Send(query);
        return View(settings);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrUpdateSiteSettingDto model)
    {
        if (model.Type == Domain.Entities.SiteSettingType.Image || 
            model.Type == Domain.Entities.SiteSettingType.Video ||
            model.Type == Domain.Entities.SiteSettingType.Zip ||
            model.Type == Domain.Entities.SiteSettingType.File)
        {
            var fileRequest = Request.Form.Files.GetFile("FileUpload");
            if (fileRequest != null && fileRequest.Length > 0)
            {
                var filePath = await SaveFileAsync(fileRequest, model.Type);
                if (!string.IsNullOrEmpty(filePath))
                {
                    model.Value = filePath;
                }
            }
        }

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var command = new CreateSiteSettingCommand { SiteSettingData = model };
            await _mediator.Send(command);
            TempData["Success"] = "تنظیم سایت با موفقیت ایجاد شد.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var setting = await _mediator.Send(new GetSiteSettingByIdQuery { SiteSettingId = id });
        if (setting == null)
            return NotFound();

        var dto = new CreateOrUpdateSiteSettingDto
        {
            Id = setting.Id,
            Key = setting.Key,
            Value = setting.Value,
            Type = setting.Type,
            SortOrder = setting.SortOrder,
            IsPublished = setting.IsPublished
        };

        return View(dto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateOrUpdateSiteSettingDto model)
    {
        if (model.Type == Domain.Entities.SiteSettingType.Image ||
            model.Type == Domain.Entities.SiteSettingType.Video ||
            model.Type == Domain.Entities.SiteSettingType.Zip ||
            model.Type == Domain.Entities.SiteSettingType.File)
        {
            var fileRequest = Request.Form.Files.GetFile("FileUpload");
            if (fileRequest != null && fileRequest.Length > 0)
            {
                var filePath = await SaveFileAsync(fileRequest, model.Type);
                if (!string.IsNullOrEmpty(filePath))
                {
                    model.Value = filePath;
                }
            }
        }

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var command = new UpdateSiteSettingCommand { SiteSettingId = id, SiteSettingData = model };
            await _mediator.Send(command);
            TempData["Success"] = "تنظیم سایت با موفقیت بروزرسانی شد.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteSiteSettingCommand { SiteSettingId = id };
        var result = await _mediator.Send(command);

        if (result)
            TempData["Success"] = "تنظیم سایت با موفقیت حذف شد.";
        else
            TempData["Error"] = "تنظیم سایت یافت نشد.";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TogglePublish(Guid id)
    {
        var command = new ToggleSiteSettingPublishCommand { SiteSettingId = id };
        var result = await _mediator.Send(command);

        if (result)
            TempData["Success"] = "وضعیت انتشار تغییر یافت.";
        else
            TempData["Error"] = "تنظیم سایت یافت نشد.";

        return RedirectToAction(nameof(Index));
    }

    private async Task<string?> SaveFileAsync(IFormFile file, Domain.Entities.SiteSettingType type)
    {
        if (file == null || file.Length == 0)
            return null;

        var uploadDir = type switch
        {
            Domain.Entities.SiteSettingType.Image => Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "settings", "images"),
            Domain.Entities.SiteSettingType.Video => Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "settings", "videos"),
            Domain.Entities.SiteSettingType.Zip => Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "settings", "zips"),
            _ => Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "settings", "files")
        };

        if (!Directory.Exists(uploadDir))
            Directory.CreateDirectory(uploadDir);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadDir, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/settings/{type.ToString().ToLower()}s/{fileName}";
    }
}
