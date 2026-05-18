using Microsoft.AspNetCore.Mvc;

namespace Company.Presentation.Controllers;

public class AboutUsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
