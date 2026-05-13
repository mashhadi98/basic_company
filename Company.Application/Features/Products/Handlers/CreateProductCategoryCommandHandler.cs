using Company.Application.Features.Products.Commands;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Repositories;
using Company.Domain.Entities;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class CreateProductCategoryCommandHandler : IRequestHandler<CreateProductCategoryCommand, ProductCategoryDto>
{
    private readonly IProductCategoryRepository _categoryRepository;

    public CreateProductCategoryCommandHandler(IProductCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductCategoryDto> Handle(CreateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var dto = request.CategoryData;

        if (dto.ParentCategoryId.HasValue)
        {
            if (dto.ParentCategoryId.Value == Guid.Empty)
                throw new InvalidOperationException("دسته‌بندی والد نامعتبر است.");

            var parentCategory = await _categoryRepository.GetByIdAsync(dto.ParentCategoryId.Value, cancellationToken);
            if (parentCategory == null)
                throw new InvalidOperationException("دسته‌بندی والد یافت نشد.");
        }

        var category = new ProductCategory
        {
            Title = dto.Title,
            Slug = dto.Slug,
            Description = dto.Description,
            ParentCategoryId = dto.ParentCategoryId,
            Image = dto.Image,
            SortOrder = dto.SortOrder,
            IsPublished = dto.IsPublished,
            SeoMetaTitle = dto.SeoMetaTitle,
            SeoMetaDescription = dto.SeoMetaDescription
        };

        await _categoryRepository.AddAsync(category, cancellationToken);

        return new ProductCategoryDto
        {
            Id = category.Id,
            Title = category.Title,
            Slug = category.Slug,
            Description = category.Description,
            ParentCategoryId = category.ParentCategoryId,
            Image = category.Image,
            SortOrder = category.SortOrder,
            IsPublished = category.IsPublished,
            SeoMetaTitle = category.SeoMetaTitle,
            SeoMetaDescription = category.SeoMetaDescription
        };
    }
}
