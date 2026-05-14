using Company.Domain.Entities;
using Company.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Company.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Company.Presentation.Controllers
{
    public class OrderRequestController : Controller
    {
        private readonly AppDbContext _db;
        public OrderRequestController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var order = new OrderRequest
            {
                PhoneNumber = model.PhoneNumber,
                FullName = model.FullName,
                Description = model.Description,
                Status = OrderRequestStatus.Registered,
                CreatedAt = DateTime.UtcNow
            };
            _db.OrderRequests.Add(order);
            await _db.SaveChangesAsync();
            TempData["Success"] = "سفارش با موفقیت ثبت شد.";
            return RedirectToAction("Index", "Home");
        }
    }
}
