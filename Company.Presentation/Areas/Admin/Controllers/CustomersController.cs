using Company.Application.Features.Customers.Commands;
using Company.Application.Features.Customers.DTOs;
using Company.Application.Features.Customers.Queries;
using Company.Presentation.Areas.Admin.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Presentation.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class CustomersController : Controller
{
    private readonly IMediator _mediator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public CustomersController(IMediator mediator, IWebHostEnvironment webHostEnvironment)
    {
        _mediator = mediator;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public async Task<IActionResult> Index(bool? isPublished, int? skip, int? take)
    {
        var customers = await _mediator.Send(new GetCustomersQuery
        {
            IsPublished = isPublished,
            Skip = skip,
            Take = take
        });

        return View(customers);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var customer = await _mediator.Send(new GetCustomerByIdQuery { CustomerId = id });
        return customer == null ? NotFound() : View(customer);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CustomerViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CustomerViewModel model)
    {
        await SaveUploadedImageAsync(model);

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var customer = await _mediator.Send(new CreateCustomerCommand
            {
                CustomerData = ToDto(model)
            });

            return RedirectToAction(nameof(Details), new { id = customer.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var customer = await _mediator.Send(new GetCustomerByIdQuery { CustomerId = id });
        if (customer == null)
            return NotFound();

        return View(new CustomerViewModel
        {
            Id = customer.Id,
            CompanyName = customer.CompanyName,
            Description = customer.Description,
            CompanyImage = customer.CompanyImage,
            SortOrder = customer.SortOrder,
            IsPublished = customer.IsPublished
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, CustomerViewModel model)
    {
        await SaveUploadedImageAsync(model);

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var customer = await _mediator.Send(new UpdateCustomerCommand
            {
                CustomerId = id,
                CustomerData = ToDto(model)
            });

            return RedirectToAction(nameof(Details), new { id = customer.Id });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteCustomerCommand { CustomerId = id });
        TempData[result ? "Success" : "Error"] = result ? "مشتری با موفقیت حذف شد." : "مشتری یافت نشد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> TogglePublish(Guid id)
    {
        var result = await _mediator.Send(new ToggleCustomerPublishCommand { CustomerId = id });
        TempData[result ? "Success" : "Error"] = result ? "وضعیت انتشار تغییر یافت." : "مشتری یافت نشد.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reorder(List<ReorderCustomersCommand.ReorderItem> items)
    {
        await _mediator.Send(new ReorderCustomersCommand { Items = items });
        return Json(new { success = true });
    }

    private async Task SaveUploadedImageAsync(CustomerViewModel model)
    {
        if (model.CompanyImageFile == null || model.CompanyImageFile.Length == 0)
            return;

        var imagePath = await SaveCustomerImageAsync(model.CompanyImageFile);
        if (!string.IsNullOrWhiteSpace(imagePath))
            model.CompanyImage = imagePath;
    }

    private async Task<string?> SaveCustomerImageAsync(IFormFile imageFile)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp", ".bmp" };
        var extension = Path.GetExtension(imageFile.FileName)?.ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
        {
            ModelState.AddModelError(string.Empty, "نوع فایل تصویر معتبر نیست. لطفا یک فایل JPG، PNG، GIF یا WEBP انتخاب کنید.");
            return null;
        }

        var imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "customers");
        if (!Directory.Exists(imagesFolder))
            Directory.CreateDirectory(imagesFolder);

        var fileName = $"customer-{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(imagesFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        return $"/images/customers/{fileName}";
    }

    private static CreateOrUpdateCustomerDto ToDto(CustomerViewModel model)
    {
        return new CreateOrUpdateCustomerDto
        {
            Id = model.Id,
            CompanyName = model.CompanyName,
            Description = model.Description,
            CompanyImage = model.CompanyImage,
            SortOrder = model.SortOrder,
            IsPublished = model.IsPublished
        };
    }
}
