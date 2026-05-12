using Company.Application.Abstractions;
using Company.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Company.Presentation.Controllers
{
    public class HomeController(IAppInfoService appInfo) : Controller
    {
        public IActionResult Index()
        {
            ViewBag.ApplicationName = appInfo.ApplicationName;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
