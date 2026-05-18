using MediatR;

namespace Company.Application.Features.Products.Queries;

public class GetProductsCountQuery : IRequest<int>
{
    public Guid? CategoryId { get; set; }
    public bool? IsPublished { get; set; }
    public string? SearchTerm { get; set; }
}
