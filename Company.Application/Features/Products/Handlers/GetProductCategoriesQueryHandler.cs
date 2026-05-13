using Company.Application.Features.Products.Queries;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Repositories;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class GetProductCategoriesQueryHandler : IRequestHandler<GetProductCategoriesQuery, List<ProductCategoryDto>>
{
    private readonly IProductCategoryRepository _categoryRepository;

    public GetProductCategoriesQueryHandler(IProductCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<ProductCategoryDto>> Handle(GetProductCategoriesQuery request, CancellationToken cancellationToken)
    {
        List<ProductCategoryDto> categories;

        if (request.ParentCategoryId.HasValue)
        {
            var childCategories = await _categoryRepository.GetChildCategoriesAsync(request.ParentCategoryId.Value, cancellationToken);
            categories = MapToDtos(childCategories);
        }
        else
        {
            var allCategories = await _categoryRepository.GetAllAsync(cancellationToken);

            if (request.IsPublished.HasValue)
                allCategories = allCategories.Where(c => c.IsPublished == request.IsPublished).ToList();

            categories = MapToDtos(allCategories);
        }

        return categories.OrderBy(c => c.SortOrder).ToList();
    }

    private List<ProductCategoryDto> MapToDtos(List<Domain.Entities.ProductCategory> categories)
    {
        var idToTitle = categories.ToDictionary(c => c.Id, c => c.Title);

        return categories.Select(c => new ProductCategoryDto
        {
            Id = c.Id,
            Title = c.Title,
            Slug = c.Slug,
            Description = c.Description,
            ParentCategoryId = c.ParentCategoryId,
            ParentCategoryTitle = c.ParentCategoryId.HasValue && idToTitle.TryGetValue(c.ParentCategoryId.Value, out var parentTitle)
                ? parentTitle
                : null,
            Image = c.Image,
            SortOrder = c.SortOrder,
            IsPublished = c.IsPublished,
            SeoMetaTitle = c.SeoMetaTitle,
            SeoMetaDescription = c.SeoMetaDescription
        }).ToList();
    }
}
