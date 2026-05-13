using Company.Application.Features.Products.Commands;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Queries;
using Company.Presentation.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProductCategoriesController : Controller
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductCategoriesController(IMediator mediator, IWebHostEnvironment webHostEnvironment)
    {
        _mediator = mediator;
        _webHostEnvironment = webHostEnvironment;
    }

    /// <summary>
    /// لیست دسته‌بندی محصولات
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var categories = await _mediator.Send(new GetProductCategoriesQuery());
        return View(categories);
    }

    /// <summary>
    /// فرم ایجاد دسته‌بندی جدید
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var parentCategories = await _mediator.Send(new GetProductCategoriesQuery());
        ViewBag.ParentCategories = parentCategories;
        return View(new ProductCategoryViewModel());
    }

    /// <summary>
    /// ذخیرهٔ دسته‌بندی جدید
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCategoryViewModel model)
    {
        if (model.CategoryImageFile != null && model.CategoryImageFile.Length > 0)
        {
            var imagePath = await SaveCategoryImageAsync(model.CategoryImageFile);
            if (!string.IsNullOrEmpty(imagePath))
            {
                model.Image = imagePath;
            }
        }

        if (!ModelState.IsValid)
        {
            var parentCategories = await _mediator.Send(new GetProductCategoriesQuery());
            ViewBag.ParentCategories = parentCategories;
            return View(model);
        }

        var dto = new CreateOrUpdateProductCategoryDto
        {
            Title = model.Title,
            Slug = model.Slug,
            Description = model.Description,
            ParentCategoryId = model.ParentCategoryId,
            Image = model.Image,
            SortOrder = model.SortOrder,
            IsPublished = model.IsPublished,
            SeoMetaTitle = model.SeoMetaTitle,
            SeoMetaDescription = model.SeoMetaDescription
        };

        try
        {
            var command = new CreateProductCategoryCommand { CategoryData = dto };
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var parentCategories = await _mediator.Send(new GetProductCategoriesQuery());
            ViewBag.ParentCategories = parentCategories;
            return View(model);
        }
    }

    /// <summary>
    /// فرم ویرایش دسته‌بندی
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var category = await _mediator.Send(new GetProductCategoryByIdQuery { CategoryId = id });
        if (category == null)
            return NotFound();

        var model = new ProductCategoryViewModel
        {
            Title = category.Title,
            Slug = category.Slug,
            Description = category.Description,
            ParentCategoryId = category.ParentCategoryId,
            Image = category.Image,
            SortOrder = category.SortOrder,
            IsPublished = category.IsPublished,
            SeoMetaTitle = category.SeoMetaTitle,
            SeoMetaDescription = category.SeoMetaDescription
        };

        var parentCategories = await _mediator.Send(new GetProductCategoriesQuery());
        ViewBag.ParentCategories = parentCategories;
        ViewBag.CategoryId = id;

        return View(model);
    }

    /// <summary>
    /// ذخیرهٔ تغییرات دسته‌بندی
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ProductCategoryViewModel model)
    {
        if (model.CategoryImageFile != null && model.CategoryImageFile.Length > 0)
        {
            var imagePath = await SaveCategoryImageAsync(model.CategoryImageFile);
            if (!string.IsNullOrEmpty(imagePath))
            {
                model.Image = imagePath;
            }
        }

        if (!ModelState.IsValid)
        {
            var parentCategories = await _mediator.Send(new GetProductCategoriesQuery());
            ViewBag.ParentCategories = parentCategories;
            ViewBag.CategoryId = id;
            return View(model);
        }

        try
        {
            var dto = new CreateOrUpdateProductCategoryDto
            {
                Title = model.Title,
                Slug = model.Slug,
                Description = model.Description,
                ParentCategoryId = model.ParentCategoryId,
                Image = model.Image,
                SortOrder = model.SortOrder,
                IsPublished = model.IsPublished,
                SeoMetaTitle = model.SeoMetaTitle,
                SeoMetaDescription = model.SeoMetaDescription
            };

            var command = new UpdateProductCategoryCommand { CategoryId = id, CategoryData = dto };
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            var parentCategories = await _mediator.Send(new GetProductCategoriesQuery());
            ViewBag.ParentCategories = parentCategories;
            ViewBag.CategoryId = id;
            return View(model);
        }
    }

    private async Task<string?> SaveCategoryImageAsync(IFormFile categoryImageFile)
    {
        if (categoryImageFile == null || categoryImageFile.Length == 0)
        {
            return null;
        }

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
        var extension = Path.GetExtension(categoryImageFile.FileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
        {
            ModelState.AddModelError(string.Empty, "نوع فایل تصویر معتبر نیست. لطفاً یک فایل JPG، PNG، GIF یا WEBP انتخاب کنید.");
            return null;
        }

        var imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "categories");
        if (!Directory.Exists(imagesFolder))
        {
            Directory.CreateDirectory(imagesFolder);
        }

        var fileName = $"category-{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(imagesFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await categoryImageFile.CopyToAsync(stream);
        }

        return $"/images/categories/{fileName}";
    }

    /// <summary>
    /// حذف دسته‌بندی
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            var command = new DeleteProductCategoryCommand { CategoryId = id };
            await _mediator.Send(command);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
            return RedirectToAction(nameof(Index));
        }
    }
}
