using System.Collections.Generic;
using Company.Domain.Entities;

namespace Company.Presentation.Areas.Admin.Models
{
    public class OrderRequestFilterViewModel
    {
        public string? PhoneNumber { get; set; }
        public string? FullName { get; set; }
        public OrderRequestStatus? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public int TotalCount { get; set; }
        public List<OrderRequest> Orders { get; set; } = new();
    }
}