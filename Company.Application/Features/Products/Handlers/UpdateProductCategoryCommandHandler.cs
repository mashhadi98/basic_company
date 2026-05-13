using Company.Application.Features.Products.Commands;
using Company.Application.Features.Products.DTOs;
using Company.Application.Features.Products.Repositories;
using Company.Domain.Entities;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class UpdateProductCategoryCommandHandler : IRequestHandler<UpdateProductCategoryCommand, ProductCategoryDto>
{
    private readonly IProductCategoryRepository _categoryRepository;

    public UpdateProductCategoryCommandHandler(IProductCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ProductCategoryDto> Handle(UpdateProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
            throw new InvalidOperationException("دسته‌بندی مورد نظر یافت نشد.");

        var dto = request.CategoryData;

        if (dto.ParentCategoryId.HasValue)
        {
            if (dto.ParentCategoryId.Value == category.Id)
                throw new InvalidOperationException("دسته‌بندی والد نمی‌تواند خودش باشد.");

            var parentCategory = await _categoryRepository.GetByIdAsync(dto.ParentCategoryId.Value, cancellationToken);
            if (parentCategory == null)
                throw new InvalidOperationException("دسته‌بندی والد یافت نشد.");
        }

        category.Title = dto.Title;
        category.Slug = dto.Slug;
        category.Description = dto.Description;
        category.ParentCategoryId = dto.ParentCategoryId;
        category.Image = dto.Image;
        category.SortOrder = dto.SortOrder;
        category.IsPublished = dto.IsPublished;
        category.SeoMetaTitle = dto.SeoMetaTitle;
        category.SeoMetaDescription = dto.SeoMetaDescription;

        await _categoryRepository.UpdateAsync(category, cancellationToken);

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
