using Company.Application.Features.Products.Commands;
using Company.Application.Features.Products.Repositories;
using MediatR;

namespace Company.Application.Features.Products.Handlers;

public class DeleteProductCategoryCommandHandler : IRequestHandler<DeleteProductCategoryCommand, bool>
{
    private readonly IProductCategoryRepository _categoryRepository;

    public DeleteProductCategoryCommandHandler(IProductCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
        if (category == null)
            return false;

        if (category.ChildCategories != null && category.ChildCategories.Any())
            throw new InvalidOperationException("ابتدا باید زیردسته‌های این دسته‌بندی حذف یا منتقل شوند.");

        await _categoryRepository.DeleteAsync(request.CategoryId, cancellationToken);
        return true;
    }
}
