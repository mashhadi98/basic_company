using Company.Application.Features.Products.Commands;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Queries;
using Company.Presentation.Areas.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Linq;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProductsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductsController(IMediator mediator, IWebHostEnvironment webHostEnvironment)
    {
        _mediator = mediator;
        _webHostEnvironment = webHostEnvironment;
    }

    /// <summary>
    /// لیست محصولات
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Index(int skip = 0, int take = 10)
    {
        var query = new GetProductsQuery { Skip = skip, Take = take };
        var products = await _mediator.Send(query);
        return View(products);
    }

    /// <summary>
    /// مشاهدهٔ جزئیات محصول
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery { ProductId = id });
        if (product == null)
            return NotFound();

        return View(product);
    }

    /// <summary>
    /// ایجاد محصول جدید
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var categories = await _mediator.Send(new GetProductCategoriesQuery { IsPublished = true });
        ViewBag.Categories = categories;
        return View(new ProductViewModel());
    }

    /// <summary>
    /// ذخیرهٔ محصول جدید
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductViewModel model)
    {
        if (model.MainImageFile != null && model.MainImageFile.Length > 0)
        {
            var imagePath = await SaveProductImageAsync(model.MainImageFile, "main");
            if (!string.IsNullOrEmpty(imagePath))
            {
                model.MainImage = imagePath;
            }
        }

        if (model.ThumbnailImageFile != null && model.ThumbnailImageFile.Length > 0)
        {
            var imagePath = await SaveProductImageAsync(model.ThumbnailImageFile, "thumbnail");
            if (!string.IsNullOrEmpty(imagePath))
            {
                model.ThumbnailImage = imagePath;
            }
        }

        if (!ModelState.IsValid)
        {
            var categories = await _mediator.Send(new GetProductCategoriesQuery { IsPublished = true });
            ViewBag.Categories = categories;
            return View(model);
        }

        var dto = new CreateOrUpdateProductDto
        {
            Title = model.Title,
            Slug = model.Slug,
            ShortDescription = model.ShortDescription,
            FullDescription = model.FullDescription,
            MainImage = model.MainImage,
            CategoryId = model.CategoryId,
            IsFeatured = model.IsFeatured,
            PublishStatus = model.PublishStatus,
            SortOrder = model.SortOrder,
            SeoMetaTitle = model.SeoMetaTitle,
            SeoMetaDescription = model.SeoMetaDescription,
            SeoKeywords = model.SeoKeywords,
            CanonicalUrl = model.CanonicalUrl,
            ThumbnailImage = model.ThumbnailImage,
            CatalogPdfFile = model.CatalogPdfFile,
            VideoUrl = model.VideoUrl,
            Attributes = model.Attributes,
            GalleryImages = model.GalleryImages,
            Tags = model.Tags
        };

        try
        {
            var command = new CreateProductCommand { ProductData = dto };
            var product = await _mediator.Send(command);
            return RedirectToAction(nameof(Details), new { id = product.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            var categories = await _mediator.Send(new GetProductCategoriesQuery { IsPublished = true });
            ViewBag.Categories = categories;
            return View(model);
        }
    }

    /// <summary>
    /// ویرایش محصول
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery { ProductId = id });
        if (product == null)
            return NotFound();

        var model = new ProductViewModel
        {
            Title = product.Title,
            Slug = product.Slug,
            ShortDescription = product.ShortDescription,
            FullDescription = product.FullDescription,
            MainImage = product.MainImage,
            CategoryId = product.CategoryId,
            IsFeatured = product.IsFeatured,
            PublishStatus = product.PublishStatus,
            SortOrder = product.SortOrder,
            SeoMetaTitle = product.SeoMetaTitle,
            SeoMetaDescription = product.SeoMetaDescription,
            SeoKeywords = product.SeoKeywords,
            CanonicalUrl = product.CanonicalUrl,
            ThumbnailImage = product.ThumbnailImage,
            CatalogPdfFile = product.CatalogPdfFile,
            VideoUrl = product.VideoUrl,
            Attributes = product.Attributes
                .Select(a => new CreateOrUpdateProductAttributeDto
                {
                    Id = a.Id,
                    Key = a.Key,
                    Value = a.Value,
                    SortOrder = a.SortOrder
                })
                .ToList()
        };

        var categories = await _mediator.Send(new GetProductCategoriesQuery());
        ViewBag.Categories = categories;
        ViewBag.ProductId = id;

        return View(model);
    }

    /// <summary>
    /// ذخیرهٔ تغییرات محصول
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ProductViewModel model)
    {
        if (model.MainImageFile != null && model.MainImageFile.Length > 0)
        {
            var imagePath = await SaveProductImageAsync(model.MainImageFile, "main");
            if (!string.IsNullOrEmpty(imagePath))
            {
                model.MainImage = imagePath;
            }
        }

        if (model.ThumbnailImageFile != null && model.ThumbnailImageFile.Length > 0)
        {
            var imagePath = await SaveProductImageAsync(model.ThumbnailImageFile, "thumbnail");
            if (!string.IsNullOrEmpty(imagePath))
            {
                model.ThumbnailImage = imagePath;
            }
        }

        if (!ModelState.IsValid)
        {
            var categories = await _mediator.Send(new GetProductCategoriesQuery());
            ViewBag.Categories = categories;
            ViewBag.ProductId = id;
            return View(model);
        }

        var dto = new CreateOrUpdateProductDto
        {
            Title = model.Title,
            Slug = model.Slug,
            ShortDescription = model.ShortDescription,
            FullDescription = model.FullDescription,
            MainImage = model.MainImage,
            CategoryId = model.CategoryId,
            IsFeatured = model.IsFeatured,
            PublishStatus = model.PublishStatus,
            SortOrder = model.SortOrder,
            SeoMetaTitle = model.SeoMetaTitle,
            SeoMetaDescription = model.SeoMetaDescription,
            SeoKeywords = model.SeoKeywords,
            CanonicalUrl = model.CanonicalUrl,
            ThumbnailImage = model.ThumbnailImage,
            CatalogPdfFile = model.CatalogPdfFile,
            VideoUrl = model.VideoUrl,
            Attributes = model.Attributes,
            GalleryImages = model.GalleryImages,
            Tags = model.Tags
        };

        try
        {
            var command = new UpdateProductCommand { ProductId = id, ProductData = dto };
            var product = await _mediator.Send(command);
            return RedirectToAction(nameof(Details), new { id = product.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            var categories = await _mediator.Send(new GetProductCategoriesQuery());
            ViewBag.Categories = categories;
            ViewBag.ProductId = id;
            return View(model);
        }
    }

    private async Task<string?> SaveProductImageAsync(IFormFile productImageFile, string type)
    {
        if (productImageFile == null || productImageFile.Length == 0)
            return null;

        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
        var extension = Path.GetExtension(productImageFile.FileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
        {
            ModelState.AddModelError(string.Empty, "نوع فایل تصویر معتبر نیست. لطفاً یک فایل JPG، PNG، GIF یا WEBP انتخاب کنید.");
            return null;
        }

        var imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
        if (!Directory.Exists(imagesFolder))
        {
            Directory.CreateDirectory(imagesFolder);
        }

        var fileName = $"product-{type}-{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(imagesFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await productImageFile.CopyToAsync(stream);
        }

        return $"/images/products/{fileName}";
    }

    /// <summary>
    /// اضافه کردن ویژگی جدید به محصول
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> AddAttribute(Guid productId, [FromBody] AddProductAttributeCommand command)
    {
        try
        {
            command.ProductId = productId;
            var attribute = await _mediator.Send(command);
            return Json(new { success = true, data = attribute });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// به‌روزرسانی ویژگی محصول
    /// </summary>
    [HttpPut]
    public async Task<IActionResult> UpdateAttribute(Guid productId, Guid attributeId, [FromBody] UpdateProductAttributeCommand command)
    {
        try
        {
            command.ProductId = productId;
            command.AttributeId = attributeId;
            var attribute = await _mediator.Send(command);
            return Json(new { success = true, data = attribute });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// حذف ویژگی محصول
    /// </summary>
    [HttpDelete]
    public async Task<IActionResult> DeleteAttribute(Guid productId, Guid attributeId)
    {
        try
        {
            var command = new DeleteProductAttributeCommand { ProductId = productId, AttributeId = attributeId };
            var result = await _mediator.Send(command);
            return Json(new { success = result });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// تغییر ترتیب ویژگی‌های محصول
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ReorderAttributes(Guid productId, [FromBody] ReorderProductAttributesCommand command)
    {
        try
        {
            command.ProductId = productId;
            var result = await _mediator.Send(command);
            return Json(new { success = result });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }

    /// <summary>
    /// API - دریافت ویژگی‌های محصول
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAttributes(Guid productId)
    {
        try
        {
            var attributes = await _mediator.Send(new GetProductAttributesQuery { ProductId = productId });
            return Json(new { success = true, data = attributes });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
}
