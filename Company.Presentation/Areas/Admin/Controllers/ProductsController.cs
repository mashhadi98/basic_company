using Company.Application.Features.Products.Commands;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Queries;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class ProductsController : Controller
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
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
        return View(new CreateOrUpdateProductDto());
    }

    /// <summary>
    /// ذخیرهٔ محصول جدید
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrUpdateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _mediator.Send(new GetProductCategoriesQuery { IsPublished = true });
            ViewBag.Categories = categories;
            return View(dto);
        }

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
            return View(dto);
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

        var dto = new CreateOrUpdateProductDto
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
                    Key = a.Key,
                    Value = a.Value,
                    SortOrder = a.SortOrder
                })
                .ToList()
        };

        var categories = await _mediator.Send(new GetProductCategoriesQuery());
        ViewBag.Categories = categories;
        ViewBag.ProductId = id;

        return View(dto);
    }

    /// <summary>
    /// ذخیرهٔ تغییرات محصول
    /// </summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CreateOrUpdateProductDto dto)
    {
        if (!ModelState.IsValid)
        {
            var categories = await _mediator.Send(new GetProductCategoriesQuery());
            ViewBag.Categories = categories;
            ViewBag.ProductId = id;
            return View(dto);
        }

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
            return View(dto);
        }
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
