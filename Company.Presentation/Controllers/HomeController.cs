using Company.Application.Abstractions;
using Company.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Company.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;
using Company.Domain.Entities;

namespace Company.Presentation.Controllers
{
    public class HomeController(IAppInfoService appInfo, AppDbContext db) : Controller
    {
        public async Task<IActionResult> Index()
        {
            ViewBag.ApplicationName = appInfo.ApplicationName;
            var since = DateTime.UtcNow.AddDays(-30);
            var orders = await db.OrderRequests
                .Where(x => x.CreatedAt >= since)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var order = await db.OrderRequests.FindAsync(id);
            if (order != null && order.Status == OrderRequestStatus.Registered)
            {
                order.Status = OrderRequestStatus.Contacted;
                await db.SaveChangesAsync();
            }
            return RedirectToAction("Index");
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
