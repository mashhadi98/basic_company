using Company.Application.Features.Products.Queries;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Repositories;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class GetProductCategoryByIdQueryHandler : IRequestHandler<GetProductCategoryByIdQuery, ProductCategoryDto?>
{
    private readonly IProductCategoryRepository _categoryRepository;

    public GetProductCategoryByIdQueryHandler(IProductCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductCategoryDto?> Handle(GetProductCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
            return null;

        var parentTitle = category.ParentCategoryId.HasValue
            ? (await _categoryRepository.GetByIdAsync(category.ParentCategoryId.Value, cancellationToken))?.Title
            : null;

        return new ProductCategoryDto
        {
            Id = category.Id,
            Title = category.Title,
            Slug = category.Slug,
            Description = category.Description,
            ParentCategoryId = category.ParentCategoryId,
            ParentCategoryTitle = parentTitle,
            Image = category.Image,
            SortOrder = category.SortOrder,
            IsPublished = category.IsPublished,
            SeoMetaTitle = category.SeoMetaTitle,
            SeoMetaDescription = category.SeoMetaDescription
        };
    }
}
