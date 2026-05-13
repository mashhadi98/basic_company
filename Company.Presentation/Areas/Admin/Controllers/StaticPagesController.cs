using Company.Application.Features.StaticPages.Commands;
using Company.Application.Features.StaticPages.DTOs;
using Company.Application.Features.StaticPages.Queries;
using Company.Presentation.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class StaticPagesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public StaticPagesController(IMediator mediator, IWebHostEnvironment webHostEnvironment)
    {
        _mediator = mediator;
        _webHostEnvironment = webHostEnvironment;
    }

    /// <summary>
    /// لیست صفحات ثابت
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(bool? isPublished)
    {
        var query = new GetStaticPagesQuery { IsPublished = isPublished };
        var pages = await _mediator.Send(query);
        return View(pages);
    }

    /// <summary>
    /// مشاهدهٔ جزئیات صفحه
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var page = await _mediator.Send(new GetStaticPageByIdQuery { PageId = id });
        if (page == null)
            return NotFound();

        return View(page);
    }

    /// <summary>
    /// ایجاد صفحهٔ جدید
    /// </summary>
    [HttpGet]
    public IActionResult Create()
    {
        return View(new StaticPageViewModel());
    }

    /// <summary>
    /// ذخیرهٔ صفحهٔ جدید
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(StaticPageViewModel model)
    {
        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            var imagePath = await SavePageImageAsync(model.ImageFile);
            if (!string.IsNullOrEmpty(imagePath))
            {
                model.Image = imagePath;
            }
        }

        if (!ModelState.IsValid)
            return View(model);

        var dto = new CreateOrUpdateStaticPageDto
        {
            Key = model.Key,
            Title = model.Title,
            Summary = model.Summary,
            Description = model.Description,
            Image = model.Image,
            IsPublished = model.IsPublished
        };

        try
        {
            var command = new CreateStaticPageCommand { PageData = dto };
            var page = await _mediator.Send(command);
            return RedirectToAction(nameof(Details), new { id = page.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    /// <summary>
    /// ویرایش صفحه
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var page = await _mediator.Send(new GetStaticPageByIdQuery { PageId = id });
        if (page == null)
            return NotFound();

        var model = new StaticPageViewModel
        {
            Id = page.Id,
            Key = page.Key,
            Title = page.Title,
            Summary = page.Summary,
            Description = page.Description,
            Image = page.Image,
            IsPublished = page.IsPublished
        };

        return View(model);
    }

    /// <summary>
    /// ذخیرهٔ تغییرات صفحه
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, StaticPageViewModel model)
    {
        if (model.ImageFile != null && model.ImageFile.Length > 0)
        {
            var imagePath = await SavePageImageAsync(model.ImageFile);
            if (!string.IsNullOrEmpty(imagePath))
            {
                model.Image = imagePath;
            }
        }

        if (!ModelState.IsValid)
            return View(model);

        var dto = new CreateOrUpdateStaticPageDto
        {
            Key = model.Key,
            Title = model.Title,
            Summary = model.Summary,
            Description = model.Description,
            Image = model.Image,
            IsPublished = model.IsPublished
        };

        try
        {
            var command = new UpdateStaticPageCommand { PageId = id, PageData = dto };
            var page = await _mediator.Send(command);
            return RedirectToAction(nameof(Details), new { id = page.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }

    /// <summary>
    /// حذف صفحه
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var command = new DeleteStaticPageCommand { PageId = id };
        var result = await _mediator.Send(command);

        if (result)
        {
            TempData["Success"] = "صفحه ثابت با موفقیت حذف شد.";
        }
        else
        {
            TempData["Error"] = "صفحه ثابت یافت نشد.";
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// تغییر وضعیت انتشار صفحه
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TogglePublish(Guid id)
    {
        try
        {
            var command = new ToggleStaticPagePublishCommand { PageId = id };
            var result = await _mediator.Send(command);

            if (result)
                TempData["Success"] = "وضعیت انتشار با موفقیت تغییر یافت.";
            else
                TempData["Error"] = "صفحه ثابت یافت نشد.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"خطا: {ex.Message}";
        }

        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// ذخیرهٔ تصویر صفحه
    /// </summary>
    private async Task<string?> SavePageImageAsync(IFormFile imageFile)
    {
        if (imageFile == null || imageFile.Length == 0)
            return null;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
        var extension = Path.GetExtension(imageFile.FileName)?.ToLowerInvariant();
        
        if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
        {
            ModelState.AddModelError(string.Empty, "نوع فایل تصویر معتبر نیست. لطفاً یک فایل JPG، PNG، GIF یا WEBP انتخاب کنید.");
            return null;
        }

        var imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "pages");
        if (!Directory.Exists(imagesFolder))
        {
            Directory.CreateDirectory(imagesFolder);
        }

        var fileName = $"page-{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(imagesFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        return $"/images/pages/{fileName}";
    }
}