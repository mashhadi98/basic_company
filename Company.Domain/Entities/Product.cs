using Company.Domain.Common;

namespace Company.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; } = string.Empty;
}
