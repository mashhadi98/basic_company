using Microsoft.AspNetCore.Mvc;

namespace Company.Presentation.Controllers;

public class BlogsController : Controller
{
    public IActionResult Index(int page = 1)
    {
        return View();
    }

    [Route("/blogs/{Id}")]
    public IActionResult Single(Guid Id)
    {
        return View();
    }
}
