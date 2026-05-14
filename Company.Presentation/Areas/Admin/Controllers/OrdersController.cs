using Company.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Company.Presentation.Areas.Admin.Models;
using System.Linq;
using System.Threading.Tasks;
using Company.Domain.Entities;

namespace Company.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly AppDbContext _db;
        public OrdersController(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(string? phoneNumber, string? fullName, OrderRequestStatus? status, int page = 1, int pageSize = 10)
        {
            var query = _db.OrderRequests.AsQueryable();
            if (!string.IsNullOrWhiteSpace(phoneNumber))
                query = query.Where(x => x.PhoneNumber.Contains(phoneNumber));
            if (!string.IsNullOrWhiteSpace(fullName))
                query = query.Where(x => x.FullName.Contains(fullName));
            if (status.HasValue)
                query = query.Where(x => x.Status == status);
            var totalCount = await System.Threading.Tasks.Task.FromResult(query.Count());
            var orders = query.OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            var model = new OrderRequestFilterViewModel
            {
                PhoneNumber = phoneNumber,
                FullName = fullName,
                Status = status,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                Orders = orders
            };
            return View(model);
        }
    }
}
