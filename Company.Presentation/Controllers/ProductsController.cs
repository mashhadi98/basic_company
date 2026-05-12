using Company.Presentation.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Presentation.Controllers;

/// <summary>
/// نمونهٔ اعمال مجوز روی اکشن‌ها و مخفی‌سازی منو در ویو (این کنترلر صرفاً نمایشی است).
/// </summary>
[Authorize]
public sealed class ProductsController : Controller
{
    [HttpGet]
    [HasPermission("Product.View")]
    public IActionResult Index()
    {
        ViewData["Title"] = "محصولات";
        return View();
    }

    [HttpGet]
    [HasPermission("Product.Create")]
    public IActionResult Create()
    {
        ViewData["Title"] = "ایجاد محصول";
        return View();
    }
}
