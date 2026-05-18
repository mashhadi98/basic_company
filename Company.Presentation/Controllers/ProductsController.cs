using Microsoft.AspNetCore.Mvc;

namespace Company.Presentation.Controllers;

public class ProductsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Route("/products/{Id}")]
    public IActionResult Single(Guid Id)
    {
        return View();
    }
}
