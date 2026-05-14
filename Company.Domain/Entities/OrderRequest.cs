using System;

namespace Company.Domain.Entities
{
    public class OrderRequest
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Description { get; set; }
        public OrderRequestStatus Status { get; set; } = OrderRequestStatus.Registered;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum OrderRequestStatus
    {
        Registered = 0,
        Contacted = 1
    }
}
