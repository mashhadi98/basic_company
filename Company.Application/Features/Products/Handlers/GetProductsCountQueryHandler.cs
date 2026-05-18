using Company.Application.Features.Products.Queries;
using Company.Application.Features.Products.Repositories;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class GetProductsCountQueryHandler : IRequestHandler<GetProductsCountQuery, int>
{
    private readonly IProductRepository _productRepository;

    public GetProductsCountQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<int> Handle(GetProductsCountQuery request, CancellationToken cancellationToken)
    {
        return await _productRepository.CountAsync(request.CategoryId, request.IsPublished, cancellationToken);
    }
}
